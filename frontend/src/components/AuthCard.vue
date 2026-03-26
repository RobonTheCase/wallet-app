<template>
  <div
    class="backdrop-blur-xl bg-white/10 border border-white/20 shadow-2xl rounded-2xl p-8 w-full max-w-md"
  >
    <h1 class="text-3xl font-semibold text-center mb-6">Wallet App</h1>

    <div class="flex mb-6 bg-white/10 rounded-lg overflow-hidden">
      <button
        @click="mode = 'login'"
        :class="mode === 'login' ? activeTab : inactiveTab"
        class="flex-1 p-2 text-sm"
      >
        Login
      </button>

      <button
        @click="mode = 'register'"
        :class="mode === 'register' ? activeTab : inactiveTab"
        class="flex-1 p-2 text-sm"
      >
        Register
      </button>
    </div>

    <div class="space-y-4">
      <input
        v-model="email"
        type="email"
        placeholder="Email"
        class="w-full p-3 rounded-lg bg-white/10 border border-white/20 placeholder-gray-300 focus:outline-none focus:ring-2 focus:ring-green-400"
      />

      <input
        v-model="password"
        type="password"
        placeholder="Password"
        class="w-full p-3 rounded-lg bg-white/10 border border-white/20 placeholder-gray-300 focus:outline-none focus:ring-2 focus:ring-green-400"
      />
    </div>

    <p v-if="error" class="text-red-400 text-sm mt-4">
      {{ error }}
    </p>

    <div class="mt-6">
      <BaseButton
        @click="handleSubmit"
        :disabled="loading"
        :class="loading ? 'opacity-50 cursor-not-allowed' : ''"
      >
        <span v-if="loading">Loading...</span>
        <span v-else>
          {{ mode === 'login' ? 'Login' : 'Register' }}
        </span>
      </BaseButton>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import BaseButton from './BaseButton.vue';

const email = ref('');
const password = ref('');
const mode = ref<'login' | 'register'>('login');
const loading = ref(false);
const error = ref('');

const handleSubmit = async () => {
  error.value = '';
  loading.value = true;

  try {
    const endpoint =
      mode.value === 'login'
        ? 'http://localhost:5172/api/auth/login'
        : 'http://localhost:5172/api/auth/register';

    const response = await fetch(endpoint, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        email: email.value,
        password: password.value,
      }),
    });

    const data = await response.json();

    if (!response.ok) {
      throw new Error(data.message || 'Something went wrong');
    }

    emit('authenticated', data.data);

    console.log('SUCCESS:', data);
  } catch (err: any) {
    error.value = err.message;
  } finally {
    loading.value = false;
  }
};

const emit = defineEmits(['authenticated']);

const activeTab = 'bg-white/20 text-white font-medium';

const inactiveTab = 'text-gray-400 hover:text-white';
</script>
