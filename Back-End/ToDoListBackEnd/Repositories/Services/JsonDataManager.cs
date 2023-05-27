using ToDoListBackEnd.Repositories.Interfaces;

using ToDoListBackEnd.Models;
using System.Text.Json;

namespace ToDoListBackEnd.Repositories.Services;

public class JsonDataManager : IJsonDataManager
{
    private readonly string _jsonFilePath = "data.json";

    public async Task<List<ToDoItem>> LoadDataFromFileAsync()
    {
        if (File.Exists(_jsonFilePath))
        {
            string json = await File.ReadAllTextAsync(_jsonFilePath);
            return JsonSerializer.Deserialize<List<ToDoItem>>(json);
        }

        return new List<ToDoItem>();
    }

    public async Task SaveDataToFileAsync(List<ToDoItem> toDoItems)
    {
        string json = JsonSerializer.Serialize(toDoItems);
        await File.WriteAllTextAsync(_jsonFilePath, json);
    }
}

