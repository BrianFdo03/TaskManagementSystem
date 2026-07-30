<script setup lang="ts">
import { useRouter } from "vue-router";
import { useAuthStore } from "@/stores/auth";

const router = useRouter();
const authStore = useAuthStore();

const handleLogout = () => {
  authStore.logout();

  router.push({
    name: "Login",
  });
};
</script>

<template>
  <div class="min-h-screen flex bg-gray-100">
    <!-- Sidebar -->
    <aside class="w-64 bg-white shadow-md">
      <div class="p-6 border-b">
        <h1 class="text-xl font-bold">
          Task Management
        </h1>
      </div>

      <nav class="p-4 space-y-2">
        <RouterLink
          to="/app/dashboard"
          class="block px-4 py-2 rounded hover:bg-gray-100"
        >
          Dashboard
        </RouterLink>

        <RouterLink
          to="/app/tasks"
          class="block px-4 py-2 rounded hover:bg-gray-100"
        >
          Tasks
        </RouterLink>

        <RouterLink
          to="/app/users"
          class="block px-4 py-2 rounded hover:bg-gray-100"
        >
          Users
        </RouterLink>
      </nav>
    </aside>

    <!-- Main Content -->
    <div class="flex-1 flex flex-col">

      <!-- Header -->
      <header
        class="bg-white shadow px-6 py-4 flex justify-between items-center"
      >
        <h2 class="text-lg font-semibold">
          Task Management System
        </h2>

        <button
          @click="handleLogout"
          class="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600"
        >
          Logout
        </button>
      </header>

      <!-- Routed Page -->
      <main class="flex-1 p-6">
        <RouterView />
      </main>

    </div>
  </div>
</template>