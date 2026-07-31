<script setup lang="ts">
import { ref } from 'vue'
import { Mail, LockKeyhole, ClipboardList, Eye, EyeOff } from 'lucide-vue-next'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const authStore = useAuthStore()

const email = ref('')
const password = ref('')
const errorMessage = ref('')
const isLoading = ref(false)
const showPassword = ref(false)

const handleLogin = async () => {
  errorMessage.value = ''

  if (!email.value.trim() || !password.value) {
    errorMessage.value = 'Please enter your email and password.'
    return
  }

  isLoading.value = true

  try {
    await authStore.login({
      email: email.value.trim(),
      password: password.value,
    })

    await router.push({
      name: 'Dashboard',
    })
  } catch (error) {
    console.error('Login failed:', error)

    errorMessage.value = 'Invalid email or password.'
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <div class="min-h-screen bg-gray-50 px-5 py-8 sm:px-6">
    <div class="flex min-h-[calc(100vh-4rem)] items-center justify-center">
      <div class="w-full max-w-md">
        <!-- Brand -->
        <div class="mb-8 text-center">
          <div
            class="mx-auto flex h-16 w-16 items-center justify-center rounded-2xl bg-blue-600 shadow-sm"
          >
            <ClipboardList class="h-8 w-8 text-white" />
          </div>

          <h1 class="mt-5 text-2xl font-semibold tracking-tight text-gray-900">Task Management</h1>

          <p class="mt-2 text-sm text-gray-500">Sign in to manage your tasks</p>
        </div>

        <!-- Login Card -->
        <div class="rounded-2xl border border-gray-100 bg-white p-6 shadow-sm sm:p-8">
          <div class="mb-6">
            <h2 class="text-lg font-semibold text-gray-900">Welcome back</h2>

            <p class="mt-1 text-sm text-gray-500">Enter your credentials to continue.</p>
          </div>

          <form @submit.prevent="handleLogin" class="space-y-5">
            <!-- Email -->
            <div>
              <label for="email" class="mb-1.5 block text-sm font-medium text-gray-700">
                Email
              </label>

              <div class="relative">
                <Mail
                  class="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-gray-400"
                />

                <input
                  id="email"
                  v-model="email"
                  type="email"
                  autocomplete="email"
                  placeholder="Enter your email"
                  class="w-full rounded-xl border border-gray-200 bg-white py-3 pl-10 pr-4 text-sm text-gray-900 outline-none transition placeholder:text-gray-400 focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
                  :disabled="isLoading"
                />
              </div>
            </div>

            <!-- Password -->
            <div>
              <label for="password" class="mb-1.5 block text-sm font-medium text-gray-700">
                Password
              </label>

              <div class="relative">
                <LockKeyhole
                  class="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-gray-400"
                />

                <input
                  id="password"
                  v-model="password"
                  :type="showPassword ? 'text' : 'password'"
                  autocomplete="off"
                  placeholder="Enter your password"
                  class="w-full rounded-xl border border-gray-200 bg-white py-3 pl-10 pr-11 text-sm text-gray-900 outline-none transition placeholder:text-gray-400 focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
                  :disabled="isLoading"
                />

                <button
                  type="button"
                  :disabled="isLoading"
                  @click="showPassword = !showPassword"
                  class="absolute right-3 top-1/2 flex h-7 w-7 -translate-y-1/2 items-center justify-center rounded-lg text-gray-400 transition hover:bg-gray-100 hover:text-gray-600 disabled:cursor-not-allowed"
                  :aria-label="showPassword ? 'Hide password' : 'Show password'"
                >
                  <EyeOff v-if="showPassword" class="h-4 w-4" />
                  <Eye v-else class="h-4 w-4" />
                </button>
              </div>
            </div>

            <!-- Error -->
            <div v-if="errorMessage" class="rounded-xl border border-red-100 bg-red-50 px-4 py-3">
              <p class="text-sm text-red-600">
                {{ errorMessage }}
              </p>
            </div>

            <!-- Login -->
            <button
              type="submit"
              :disabled="isLoading"
              class="flex w-full items-center justify-center rounded-xl bg-blue-600 px-4 py-3 text-sm font-semibold text-white shadow-sm transition hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-60"
            >
              <span v-if="isLoading"> Logging in... </span>

              <span v-else> Login </span>
            </button>
          </form>
        </div>

        <!-- Footer -->
        <p class="mt-6 text-center text-xs text-gray-400">Task Management System</p>
      </div>
    </div>
  </div>
</template>
<style scoped>
/* Hide Microsoft Edge's built-in password reveal button */
input[type='password']::-ms-reveal,
input[type='password']::-ms-clear {
  display: none;
}
</style>
