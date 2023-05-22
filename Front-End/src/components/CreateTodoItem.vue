<script setup lang="ts">
import { reactive } from 'vue';
import { useTodoStore } from '@/stores/TodoStore';

const todoStore = useTodoStore();

const createTodoItem = reactive({
  name: '',
});
</script>

<template>
  <div>
      <h1>Create Todo Item</h1>
      <form @submit.prevent="todoStore.createTodoItemAsync(createTodoItem.name)">
        <div>
          <span>Nome</span>
          <input type="text" maxlength="20" v-model="createTodoItem.name" required>
        </div>
        <button type="submit">Add</button>
      </form>
    </div>
</template>

<style scoped>
</style>
