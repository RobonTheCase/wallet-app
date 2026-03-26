<template>
  <div
    class="min-h-screen bg-gradient-to-br from-slate-900 via-gray-900 to-black text-white flex items-center justify-center"
  >
    <AuthCard v-if="!user" @authenticated="setUser" />

    <Dashboard v-else :user="user" />
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import AuthCard from './components/AuthCard.vue';
import Dashboard from './components/Dashboard.vue';

const user = ref<any>(null);

const setUser = (u: any) => {
  user.value = u;
};
</script>
