const https = require("https");
const axios = require("axios");

// Create an instance of an HTTPS agent with verification disabled
const httpsAgent = new https.Agent({
  rejectUnauthorized: false,
});

const api = axios.create({
  baseURL: "https://localhost:7184/api",
  httpsAgent,
});

async function getAllTodoItemsAsync() {
  try {
    const response = await api.get("/ToDoItems");
    console.log(response);
  } catch (error) {
    console.error(error);
  }
}

async function getTodoItemByIdAsync(id) {
  try {
    const response = await api.get(`/ToDoItems/${id}`);
    console.log(response);
  } catch (error) {
    console.error(error);
  }
}

async function putTodoItemAsync(id, todoItem) {
  try {
    const response = await api.put(`/ToDoItems/${id}`, {
      Id: id,
      Name: todoItem.Name,
      IsComplete: todoItem.IsComplete,
    });
    console.log(response);
  } catch (error) {
    console.error(error);
  }
}

async function postTodoItemAsync(todoItem) {
  try {
    const response = await api.post("/ToDoItems", {
      Name: todoItem.Name,
      IsComplete: todoItem.IsComplete,
    });
    console.log(response);
  } catch (error) {
    console.error(error);
  }
}

async function deleteTodoItemByIdAsync(id) {
  try {
    const response = await api.delete(`/ToDoItems/${id}`);
    console.log(response);
  } catch (error) {
    console.error(error);
  }
}

const todoItem = {
  Name: "Test name",
  IsComplete: false,
};

const updatedTodoItem = {
  Name: "Updated name",
  IsComplete: true,
};

//getAllTodoItems();
//getTodoItemById(1);
//putTodoItem(1, updatedTodoItem);
//postTodoItem(todoItem);
//deleteTodoItemById(1);
