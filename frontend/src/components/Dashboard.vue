<template>
  <div class="relative w-full max-w-md">
    <div class="absolute inset-0 bg-green-500/10 blur-2xl rounded-2xl"></div>

    <div
      class="relative backdrop-blur-xl bg-white/10 border border-white/20 shadow-2xl rounded-2xl p-8"
    >
      <div class="mb-6">
        <p class="text-gray-400 text-xs mb-2">Current Balance</p>

        <div class="flex items-center gap-3">
          <span class="text-4xl font-semibold tracking-tight">
            R {{ balance }}
          </span>
        </div>
      </div>

      <p class="text-xs text-gray-400 mb-6">
        {{ user.email }}
      </p>

      <input
        v-model="amount"
        type="number"
        placeholder="Enter amount"
        class="w-full p-3 rounded-lg bg-white/10 border border-white/20 placeholder-gray-400 text-white focus:outline-none focus:ring-2 focus:ring-green-400 transition mb-4"
      />

      <p v-if="error" class="text-red-400 text-sm mb-2">
        {{ error }}
      </p>

      <div class="flex gap-3 mt-4">
        <button
          @click="deposit"
          class="flex-1 p-3 rounded-xl font-semibold bg-gradient-to-r from-green-400 to-emerald-500 text-black shadow-lg shadow-green-500/20 hover:scale-[1.03] hover:shadow-green-400/40 active:scale-[0.98] transition-all duration-200"
          :class="loading ? 'opacity-50 cursor-not-allowed' : ''"
          :disabled="loading"
        >
          Deposit
        </button>

        <button
          @click="withdraw"
          class="flex-1 p-3 rounded-xl font-semibold bg-white/5 border border-white/20 text-white hover:bg-white/10 hover:border-white/30 active:scale-[0.98] transition-all duration-200"
          :class="loading ? 'opacity-50 cursor-not-allowed' : ''"
          :disabled="loading"
        >
          Withdraw
        </button>
      </div>

      <div class="mt-8">
        <p class="text-gray-400 text-xs mb-3">Recent Transactions</p>

        <div class="space-y-2 max-h-48 overflow-y-auto pr-1">
          <div
            v-for="(t, i) in transactions"
            :key="i"
            class="flex justify-between items-center px-4 py-3 rounded-xl bg-white/5 border border-white/10 hover:bg-white/10 transition"
          >
            <div>
              <p class="text-sm font-medium">
                {{ t.type === 'CREDIT' ? 'Deposit' : 'Withdraw' }}
              </p>
              <p class="text-xs text-gray-400">
                {{ t.createdAt }}
              </p>
            </div>

            <p
              class="text-sm font-semibold"
              :class="t.type === 'CREDIT' ? 'text-green-400' : 'text-red-400'"
            >
              {{ t.type === 'CREDIT' ? '+' : '-' }} R {{ t.amount }}
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';

const props = defineProps<{ user: any }>();

const balance = ref(props.user.balance);

const animateBalance = (newVal: number) => {
  const start = balance.value;
  const duration = 300;
  const startTime = performance.now();

  const step = (currentTime: number) => {
    const progress = Math.min((currentTime - startTime) / duration, 1);
    balance.value = Math.floor(start + (newVal - start) * progress);

    if (progress < 1) {
      requestAnimationFrame(step);
    }
  };

  requestAnimationFrame(step);
};

const transactions = ref<any[]>([]);

const amount = ref(0);

const loading = ref(false);
const error = ref('');

const deposit = async () => {
  if (!amount.value) return;

  loading.value = true;
  error.value = '';

  try {
    const response = await fetch('http://localhost:5172/api/wallet/deposit', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        userId: props.user.id,
        amount: amount.value,
      }),
    });

    const data = await response.json();

    if (!response.ok) {
      throw new Error(data.message || 'Deposit failed');
    }

    animateBalance(data.data.balance);
    amount.value = 0;
    await fetchTransactions();
  } catch (err: any) {
    error.value = err.message;
  } finally {
    loading.value = false;
  }
};

const withdraw = async () => {
  if (!amount.value) return;

  loading.value = true;
  error.value = '';

  try {
    const response = await fetch('http://localhost:5172/api/wallet/withdraw', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        userId: props.user.id,
        amount: amount.value,
      }),
    });

    const data = await response.json();

    if (!response.ok) {
      throw new Error(data.message || 'Withdraw failed');
    }

    animateBalance(data.data.balance);
    amount.value = 0;
    await fetchTransactions();
  } catch (err: any) {
    error.value = err.message;
  } finally {
    loading.value = false;
  }
};

const fetchTransactions = async () => {
  try {
    const res = await fetch(
      `http://localhost:5172/api/wallet/transactions/${props.user.id}`,
    );
    const data = await res.json();

    if (data.success) {
      transactions.value = data.data;
    }
  } catch (err) {
    console.error(err);
  }
};

onMounted(() => {
  fetchTransactions();
});
</script>
