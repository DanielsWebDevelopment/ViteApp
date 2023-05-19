const axios = require("axios");

const api = axios.create({
  baseURL: "https://localhost:7184/api",
});

interface TodoItem {
  name: string;
  isComplete: boolean;
}

async function getAllTodoItemsAsync() {
  try {
    const response = await api.get("/ToDoItems");
    console.log(response);
  } catch (error) {
    console.error("Failed to get all todo items:", error);
  }
}

async function getTodoItemByIdAsync(id: number) {
  try {
    const response = await api.get(`/ToDoItems/${id}`);
    console.log(response);
  } catch (error) {
    console.error(`Failed to get todo item with id ${id}:`, error);
  }
}

async function putTodoItemAsync(id: number, todoItem: TodoItem) {
  try {
    const response = await api.put(`/ToDoItems/${id}`, {
      Id: id,
      Name: todoItem.name,
      IsComplete: todoItem.isComplete,
    });
    console.log(response);
  } catch (error) {
    console.error(`Failed to update todo item with id ${id}:`, error);
  }
}

async function postTodoItemAsync(todoItem: TodoItem) {
  try {
    const response = await api.post("/ToDoItems", {
      Name: todoItem.name,
      IsComplete: todoItem.isComplete,
    });
    console.log(response);
  } catch (error) {
    console.error("Failed to create new todo item:", error);
  }
}

async function deleteTodoItemByIdAsync(id: number) {
  try {
    const response = await api.delete(`/ToDoItems/${id}`);
    console.log(response);
  } catch (error) {
    console.error(`Failed to delete todo item with id ${id}:`, error);
  }
}

const todoItem: TodoItem = {
  name: "Test name",
  isComplete: false,
};

const updatedTodoItem: TodoItem = {
  name: "Updated name",
  isComplete: true,
};

// tsc \\
getAllTodoItemsAsync();
//getTodoItemByIdAsync(1);
//putTodoItemAsync(1, updatedTodoItem);
//postTodoItemAsync(todoItem);
//deleteTodoItemByIdAsync(1);
