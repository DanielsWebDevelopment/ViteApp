using System;
using ToDoListBackEnd.Models;

namespace ToDoListBackEnd.Repositories.Interfaces;
public interface IJsonDataManager
{
    Task<List<ToDoItem>> LoadDataFromFileAsync();
    Task SaveDataToFileAsync(List<ToDoItem> toDoItems);
}

