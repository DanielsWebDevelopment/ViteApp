<script setup lang="ts">
import { ref } from 'vue';

const props = defineProps({
  name: String
});

const name = ref(props.name);

const emit = defineEmits(['response']);

async function updateName() {
    emit('response', name);
};
</script>

<template>
    <td class="row-name"><input type="text" name="" id="" maxlength="20" @blur="updateName()" v-model="name"></td>
</template>

<style scoped>
input {
    border: none;
    outline: none;
}
</style>