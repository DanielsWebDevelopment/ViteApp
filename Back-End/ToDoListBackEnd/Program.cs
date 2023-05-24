using Serilog;
using System.Diagnostics;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173") 
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});


// Configure Serilog to write to a log file
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Async(x =>
    {
        x.File("logs/log.txt", rollingInterval: RollingInterval.Day,
                               rollOnFileSizeLimit: true,
                               retainedFileCountLimit: 31,
                               shared: true,
                               flushToDiskInterval: TimeSpan.FromSeconds(0.5));
    }).CreateLogger();

// Configure the logging factory
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(dispose: true);

var app = builder.Build();

// Use the middleware to log IP and request details
app.Use(async (context, next) =>
{
    // Get the remote IP address
    var ipAddress = context.Connection.RemoteIpAddress;

    // Get request details, such as URL, HTTP method, headers, and query parameters
    var requestUrl = context.Request.Path;
    var httpMethod = context.Request.Method;
    var headers = JsonConvert.SerializeObject(context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()));
    var queryParameters = JsonConvert.SerializeObject(context.Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString()));

    // Start a stopwatch to measure the elapsed time
    var stopwatch = new Stopwatch();
    stopwatch.Start();

    // Invoke the next middleware in the pipeline
    await next.Invoke();

    // Stop the stopwatch and calculate the elapsed time
    stopwatch.Stop();
    var elapsedTime = stopwatch.ElapsedMilliseconds;

    // Get the response status code
    var statusCode = context.Response.StatusCode;

    // Log the IP address and request details along with other relevant information
    Log.Information($"Request from IP: {ipAddress}, URL: {requestUrl}, Method: {httpMethod}, Headers: {headers}, Query Parameters: {queryParameters}, Status Code: {statusCode}, Elapsed Time: {elapsedTime}ms");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

