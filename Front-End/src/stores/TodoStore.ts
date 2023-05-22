import { ref } from "vue";
import { defineStore } from "pinia";
import axios from "axios";

interface Todo {
  id: number,
  name: string,
  isComplete: boolean
}

const api = axios.create({
  baseURL: "https://localhost:7184/api",
});

export const useTodoStore = defineStore("todo", () => {
  const todos = ref<Todo[]>([]);

  async function getAllTodoItemsAsync() {
    try {
      todos.value = (await api.get("/ToDoItems")).data;
    } catch (error) {
      console.error(error);
    }
  }

  async function updateTodoItemAsync(todo: Todo) {
    try {
      await api.put(`/ToDoItems/${todo.id}`, {
        Id: todo.id,
        Name: todo.name,
        IsComplete: !todo.isComplete,
      });
    } catch (error) {
      console.error(error);
    } finally {
      await getAllTodoItemsAsync();
    }
  }

  async function createTodoItemAsync(name: string) {
    try {
      await api.post("/ToDoItems", {
        Name: name,
        IsComplete: false,
      });
      console.log("ok");
    } catch (error) {
      console.error(error);
    } finally {
      await getAllTodoItemsAsync();
    }
  }

  async function deleteTodoItemByIdAsync(id: number) {
    try {
      await api.delete(`/ToDoItems/${id}`);
    } catch (error) {
      console.error(error);
    } finally {
      await getAllTodoItemsAsync();
    }
  }

  return {
    todos,
    getAllTodoItemsAsync,
    updateTodoItemAsync,
    createTodoItemAsync,
    deleteTodoItemByIdAsync,
  };
});
