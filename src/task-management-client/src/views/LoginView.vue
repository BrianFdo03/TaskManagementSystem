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
import { ref } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "@/stores/auth";

const router = useRouter();
const authStore = useAuthStore();

const email = ref("");
const password = ref("");
const errorMessage = ref("");
const isLoading = ref(false);

const handleLogin = async () => {
    errorMessage.value = "";
    isLoading.value = true;

    try {
        await authStore.login({
            email: email.value,
            password: password.value,
        });

        console.log("Login successful");
        await router.push({
            name: "Dashboard",
        });

    } catch (error) {
        console.error("Login failed:", error);

        errorMessage.value =
            "Invalid email or password.";
    } finally {
        isLoading.value = false;
    }
};
</script>

<template>
    <div>
        <h1>Login</h1>

        <form @submit.prevent="handleLogin">
            <div>
                <label for="email">Email</label>

                <input
                    id="email"
                    v-model="email"
                    type="email"
                    autocomplete="email"
                />
            </div>

            <div>
                <label for="password">Password</label>

                <input
                    id="password"
                    v-model="password"
                    type="password"
                    autocomplete="current-password"
                />
            </div>

            <p v-if="errorMessage">
                {{ errorMessage }}
            </p>

            <button
                type="submit"
                :disabled="isLoading"
            >
                {{ isLoading ? "Logging in..." : "Login" }}
            </button>
        </form>
    </div>
</template>