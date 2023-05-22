<script setup lang="ts">
import createTodoItem from './components/CreateTodoItem.vue';
import showTable from './components/ShowTable.vue';
</script>

<template>
  <header>
    <h1>To Do List</h1>
  </header>
  <main>
    <createTodoItem></createTodoItem>
    <showTable></showTable>
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

footer {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 10px;
}
</style>
