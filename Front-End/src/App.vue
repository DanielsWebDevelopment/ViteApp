<script setup lang="ts">
import { ref, reactive, onMounted, computed } from 'vue';
import axios from 'axios';

interface Todo {
  id: number;
  name: string;
  isComplete: boolean;
};

const api = axios.create({
  baseURL: "https://localhost:7184/api",
});

const todos = ref<Todo[]>([]);
const hideCompleted = ref(false);

const filteredTodos = computed<Todo[]>(() => {
  return hideCompleted.value
    ? todos.value.filter((t) => !t.isComplete)
    : todos.value;
});

const postTodoItem = reactive({
  Name: '',
});

async function getAllTodoItemsAsync() {
  try {
    todos.value = (await api.get("/ToDoItems")).data;
  } catch (error) {
    console.error(error);
  }
};

async function putTodoItemAsync(todo: Todo) {
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
};

async function postTodoItemAsync() {
  try {
    await api.post("/ToDoItems", {
      Name: postTodoItem.Name,
      IsComplete: false,
    });
  } catch (error) {
    console.error(error);
  } finally {
    postTodoItem.Name = '';
    await getAllTodoItemsAsync();
  }
};

async function deleteTodoItemByIdAsync(id: number) {
  try {
    await api.delete(`/ToDoItems/${id}`);
  } catch (error) {
    console.error(error);
  } finally {
    await getAllTodoItemsAsync();
  }
};

onMounted(getAllTodoItemsAsync);
</script>

<template>
  <header>
    <h1>To Do List</h1>
  </header>
  <main>
    <div>
      <h1>Create Todo Item</h1>
      <form @submit.prevent="postTodoItemAsync">
        <div>
          <span>Nome</span>
          <input type="text" maxlength="20" v-model="postTodoItem.Name" required>
        </div>
        <button type="submit">Adicionar</button>
      </form>
    </div>

    <div class="table-container">
      <table class="todo-table">
        <thead>
          <tr>
            <th>Name</th>
            <th>Is complete?</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="todo in filteredTodos" :key="todo.id">
            <td class="row-name">{{ todo.name }}</td>
            <td><input type="checkbox" :checked="todo.isComplete" @change="putTodoItemAsync(todo)"></td>
            <td>
              <button @click="deleteTodoItemByIdAsync(todo.id)">X</button>
            </td>
          </tr>
        </tbody>
      </table>
      <button @click="hideCompleted = !hideCompleted">
        {{ hideCompleted ? 'Show all' : 'Hide completed' }}
      </button>
    </div>
  </main>
  <footer>
    &copy; 2023 PedroHenriqueW
  </footer>
</template>

<style scoped>
header {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 10px;
}

main {
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: flex-start;
  flex: 1;
  padding: 10px;
}

.table-container {
  min-width: 400px;
  max-width: 600px;
}

.todo-table {
  width: 100%;
  border-collapse: collapse;
  margin-bottom: 20px;
}

.todo-table th,
.todo-table td {
  padding: 10px;
  text-align: left;
  border-bottom: 1px solid #ccc;
}

.todo-table th {
  background-color: #f0f0f0;
  font-weight: bold;
}

.todo-table tbody tr:hover {
  background-color: #f9f9f9;
}

footer {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 10px;
}
</style>
