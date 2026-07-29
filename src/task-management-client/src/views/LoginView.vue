<!-- <template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100">
    <div class="w-full max-w-md rounded-xl bg-white p-8 shadow-md">
      <h1 class="text-2xl font-bold text-gray-900">
        Task Management System
      </h1>

      <p class="mt-2 text-gray-600">
        Login page
      </p>
    </div>
  </div>
</template> -->

<script setup lang="ts">
import api from "@/services/api";

const testConnection = async () => {
    try {
        const response = await api.get("/auth/test");

        console.log("Backend response:", response.data);
    } catch (error) {
        console.error("Backend connection failed:", error);
    }
};
</script>

<template>
    <div>
        <h1>Login</h1>

        <button @click="testConnection">
            Test Backend Connection
        </button>
    </div>
</template>