<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useTodoStore } from '@/stores/TodoStore';
import RowName from './RowName.vue';

const todoStore = useTodoStore();
const hideCompleted = ref(false);

interface Todo {
    id: number,
    name: string,
    isComplete: boolean
};

const filteredTodos = computed<Todo[]>(() => {
    return hideCompleted.value
        ? todoStore.todos.filter(todo => !todo.isComplete)
        : todoStore.todos;
});

async function updateTodoNameAsync(todo: Todo, newName: string) {
    todo.name = newName;
    await todoStore.updateTodoNameAsync(todo);
};

onMounted(todoStore.getAllTodoItemsAsync);
</script>

<template>
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
                    <RowName :name="todo.name" @response="(newName:any) => updateTodoNameAsync(todo, newName)"></RowName>
                    <td><input type="checkbox" :checked="todo.isComplete" @change="todoStore.updateTodoItemAsync(todo)"></td>
                    <td>
                        <button @click="todoStore.deleteTodoItemByIdAsync(todo.id)">X</button>
                    </td>
                </tr>
            </tbody>
        </table>
        <button @click="hideCompleted = !hideCompleted">
            {{ hideCompleted ? 'Show all' : 'Hide completed' }}
        </button>
    </div>
</template>

<style scoped>
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
