<script setup>
import { ref, reactive, onMounted, computed } from 'vue';
import axios from 'axios'

const api = axios.create({
  baseURL: "https://localhost:7184/api",
});

const itemId = ref(1);
const todos = ref([]);
const responseGetById = ref(null);
const hideCompleted = ref(false)

const filteredTodos = computed(() => {
  return hideCompleted.value
    ? todos.value.filter((t) => !t.isComplete)
    : todos.value
})

const putTodoItem = reactive({
  Id: itemId.value,
  Name: '',
  IsComplete: false,
});

const postTodoItem = reactive({
  Name: '',
  IsComplete: false,
});

async function getAllTodoItemsAsync() {
  try {
    const response = await api.get("/ToDoItems");
    todos.value = response.data;
  } catch (error) {
    console.error(error);
  }
}

async function getTodoItemByIdAsync(id) {
  try {
    const response = await api.get(`/ToDoItems/${id}`);
    responseGetById.value = response.data;
  } catch (error) {
    console.error(error);
  }
}

async function putTodoItemAsync() {
  try {
    await api.put(`/ToDoItems/${itemId.value}`, {
      Id: putTodoItem.Id,
      Name: putTodoItem.Name,
      IsComplete: putTodoItem.IsComplete,
    });
  } catch (error) {
    console.error(error);
  } finally {
    resetPutTodoItem();
    getAllTodoItemsAsync();
  }
}

async function postTodoItemAsync() {
  try {
    await api.post("/ToDoItems", {
      Name: postTodoItem.Name,
      IsComplete: postTodoItem.IsComplete,
    });
  } catch (error) {
    console.error(error);
  } finally {
    resetPostTodoItem();
    getAllTodoItemsAsync();
  }
}

async function deleteTodoItemByIdAsync(id) {
  try {
    await api.delete(`/ToDoItems/${id}`);
  } catch (error) {
    console.error(error);
  } finally {
    itemId.value++;
    getAllTodoItemsAsync();
  }
}

async function updateTodoStatus(todo) {
  putTodoItem.Id = todo.id;
  putTodoItem.IsComplete = !todo.isComplete;
  await putTodoItemAsync()
}

function resetPutTodoItem() {
  putTodoItem.Id = itemId.value;
  putTodoItem.Name = '';
  putTodoItem.IsComplete = false;
}

function resetPostTodoItem() {
  postTodoItem.Name = '';
  postTodoItem.IsComplete = false;
}

onMounted(getAllTodoItemsAsync());

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

        <div>
          <span>IsComplete?</span>
          <input type="checkbox" v-model="postTodoItem.IsComplete">
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
            <td><input type="checkbox" :checked="todo.isComplete" @change="updateTodoStatus(todo)"></td>
            <td>
              <button>Read</button> |
              <button>Update</button> |
              <button @click="deleteTodoItemByIdAsync(todo.id)">Delete</button>
            </td>
          </tr>
        </tbody>
      </table>
      <button @click="hideCompleted = !hideCompleted">
        {{ hideCompleted ? 'Show all' : 'Hide completed' }}
      </button>
    </div>

    <!-- <div>
        <form @submit.prevent="getTodoItemByIdAsync(itemId)">
          <input type="number" min="1" max="100" v-model="itemId">
          <button type="submit">Buscar Por id</button>
        </form>
        <div> {{ responseGetById }} </div>
      </div> -->

    <!-- <div>
        <h1>putTodoItemAsync</h1>
        <form @submit.prevent="putTodoItemAsync">
          <div>
            <span>Especifique o todo item por Id</span>
            <input type="number" min="1" v-model="itemId">
          </div>

          <div>
            <span>Novo Nome</span>
            <input type="text" maxlength="100" v-model="putTodoItem.Name" required>
          </div>

          <div>
            <span>IsComplete?</span>
            <input type="checkbox" v-model="putTodoItem.IsComplete">
          </div>

          <button type="submit">Modificar</button>
        </form>
      </div> -->

    <!-- <div>
        <h1>deleteTodoItemByIdAsync</h1>
        <form @submit.prevent="deleteTodoItemByIdAsync">
          <input type="number" min="1" max="100" v-model="itemId">
          <button type="submit">Deletar</button>
        </form>
      </div> -->
  </main>
  <footer>
    &copy; 2023 PedroHenriqueW
  </footer>
</template>

<style scoped>
main {
  display: flex;
  flex-direction: column;
  padding: 30px;
  gap: 10px;
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
</style>
