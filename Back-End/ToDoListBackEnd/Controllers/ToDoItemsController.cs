using Microsoft.AspNetCore.Mvc;
using ToDoListBackEnd.Data;
using ToDoListBackEnd.Models;

namespace ToDoListBackEnd.Controllers;

[Route("api/ToDoItems")]
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private readonly JsonDataManager _jsonDataManager;

    public ToDoItemsController()
    {
        _jsonDataManager = new JsonDataManager();
    }

    // GET: api/ToDoItems
    [HttpGet]
    public async Task<IActionResult> GetToDoItems()
    {
        var toDoItems = await _jsonDataManager.LoadDataFromFileAsync();
        return Ok(toDoItems);
    }

    // GET: api/ToDoItems/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetToDoItem(long id)
    {
        var toDoItems = await _jsonDataManager.LoadDataFromFileAsync();
        var toDoItem = toDoItems.FirstOrDefault(item => item.Id == id);

        if (toDoItem == null)
        {
            return NotFound();
        }

        return Ok(toDoItem);
    }

    // PUT: api/ToDoItems/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutToDoItem(long id, ToDoItem toDoItem)
    {
        var toDoItems = await _jsonDataManager.LoadDataFromFileAsync();
        var existingItem = toDoItems.FirstOrDefault(item => item.Id == id);

        if (existingItem == null)
        {
            return NotFound();
        }

        existingItem.Name = toDoItem.Name;
        existingItem.IsComplete = toDoItem.IsComplete;

        await _jsonDataManager.SaveDataToFileAsync(toDoItems);

        return NoContent();
    }

    // POST: api/ToDoItems
    [HttpPost]
    public async Task<IActionResult> PostToDoItem(ToDoItem toDoItem)
    {
        var toDoItems = await _jsonDataManager.LoadDataFromFileAsync();
        toDoItem.Id = GenerateNewId(toDoItems);
        toDoItems.Add(toDoItem);

        await _jsonDataManager.SaveDataToFileAsync(toDoItems);

        return CreatedAtAction(nameof(GetToDoItem), new { id = toDoItem.Id }, toDoItem);
    }

    // DELETE: api/ToDoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteToDoItem(long id)
    {
        var toDoItems = await _jsonDataManager.LoadDataFromFileAsync();
        var toDoItem = toDoItems.FirstOrDefault(item => item.Id == id);

        if (toDoItem == null)
        {
            return NotFound();
        }

        toDoItems.Remove(toDoItem);

        await _jsonDataManager.SaveDataToFileAsync(toDoItems);

        return NoContent();
    }

    private static long GenerateNewId(List<ToDoItem> toDoItems)
    {
        if (toDoItems.Count > 0)
        {
            return toDoItems.Max(item => item.Id) + 1;
        }
        else
        {
            return 1;
        }
    }
}

