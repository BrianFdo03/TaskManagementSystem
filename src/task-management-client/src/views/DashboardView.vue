```vue
<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import {
  CheckCircle2,
  Circle,
  ClipboardList,
  Clock3,
  Users,
  AlertTriangle,
  RefreshCw,
} from 'lucide-vue-next'

import { useAuthStore } from '@/stores/auth'
import dashboardService from '@/services/dashboardService'
import type { Dashboard } from '@/types/dashboard'

const authStore = useAuthStore()

const dashboard = ref<Dashboard | null>(null)

const loading = ref(true)
const error = ref('')

const isAdmin = computed(() => {
  return authStore.user?.role === 'Admin'
})

const loadDashboard = async () => {
  loading.value = true
  error.value = ''

  try {
    dashboard.value = await dashboardService.getDashboard()
  } catch (err) {
    console.error(err)

    error.value = 'Unable to load dashboard data.'
  } finally {
    loading.value = false
  }
}

const userName = computed(() => {
  return authStore.user?.name || 'there'
})

onMounted(() => {
  loadDashboard()
})
</script>

<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex items-start justify-between gap-4">
      <div>
        <p class="text-sm font-medium text-blue-600">
          {{ isAdmin ? 'Admin Dashboard' : 'My Dashboard' }}
        </p>

        <h1 class="mt-1 text-2xl font-semibold tracking-tight text-gray-900">
          Welcome, {{ userName }}
        </h1>

        <p class="mt-1 text-sm text-gray-500">
          {{
            isAdmin
              ? 'Here is an overview of your task management system.'
              : 'Here is an overview of your tasks.'
          }}
        </p>
      </div>

      <!-- Refresh -->
      <button
        type="button"
        @click="loadDashboard"
        :disabled="loading"
        class="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl border border-gray-200 bg-white text-gray-500 transition hover:border-blue-200 hover:bg-blue-50 hover:text-blue-600 disabled:cursor-not-allowed disabled:opacity-50"
        title="Refresh dashboard"
      >
        <RefreshCw class="h-4 w-4" :class="{ 'animate-spin': loading }" />
      </button>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="grid grid-cols-2 gap-4 lg:grid-cols-4">
      <div
        v-for="index in isAdmin ? 6 : 5"
        :key="index"
        class="h-32 animate-pulse rounded-2xl border border-gray-100 bg-white"
      ></div>
    </div>

    <!-- Error -->
    <div v-else-if="error" class="rounded-2xl border border-red-100 bg-red-50 p-5">
      <div class="flex items-start gap-3">
        <AlertTriangle class="mt-0.5 h-5 w-5 shrink-0 text-red-500" />

        <div class="flex-1">
          <h2 class="text-sm font-semibold text-red-700">Something went wrong</h2>

          <p class="mt-1 text-sm text-red-600">
            {{ error }}
          </p>

          <button
            type="button"
            @click="loadDashboard"
            class="mt-3 text-sm font-medium text-red-700 underline underline-offset-2"
          >
            Try again
          </button>
        </div>
      </div>
    </div>

    <!-- Dashboard content -->
    <template v-else-if="dashboard">
      <!-- Admin: Total Users -->
      <section v-if="isAdmin">
        <div class="rounded-2xl border border-gray-100 bg-white p-5 shadow-sm">
          <div class="flex items-center justify-between">
            <div>
              <p class="text-sm font-medium text-gray-500">Total Users</p>

              <p class="mt-2 text-3xl font-semibold text-gray-900">
                {{ dashboard.totalUsers }}
              </p>

              <p class="mt-1 text-xs text-gray-400">Users in the system</p>
            </div>

            <div class="flex h-11 w-11 items-center justify-center rounded-xl bg-blue-50">
              <Users class="h-5 w-5 text-blue-600" />
            </div>
          </div>
        </div>
      </section>

      <!-- Main statistics -->
      <section>
        <div class="grid grid-cols-2 gap-4 lg:grid-cols-4">
          <!-- Total Tasks -->
          <div class="rounded-2xl border border-gray-100 bg-white p-5 shadow-sm">
            <div class="flex items-start justify-between gap-3">
              <div>
                <p class="text-sm font-medium text-gray-500">Total Tasks</p>

                <p class="mt-2 text-3xl font-semibold text-gray-900">
                  {{ dashboard.totalTasks }}
                </p>
              </div>

              <div class="flex h-10 w-10 items-center justify-center rounded-xl bg-blue-50">
                <ClipboardList class="h-5 w-5 text-blue-600" />
              </div>
            </div>
          </div>

          <!-- Pending -->
          <div class="rounded-2xl border border-gray-100 bg-white p-5 shadow-sm">
            <div class="flex items-start justify-between gap-3">
              <div>
                <p class="text-sm font-medium text-gray-500">Pending</p>

                <p class="mt-2 text-3xl font-semibold text-gray-900">
                  {{ dashboard.pendingTasks }}
                </p>
              </div>

              <div class="flex h-10 w-10 items-center justify-center rounded-xl bg-gray-100">
                <Circle class="h-5 w-5 text-gray-500" />
              </div>
            </div>
          </div>

          <!-- In Progress -->
          <div class="rounded-2xl border border-gray-100 bg-white p-5 shadow-sm">
            <div class="flex items-start justify-between gap-3">
              <div>
                <p class="text-sm font-medium text-gray-500">In Progress</p>

                <p class="mt-2 text-3xl font-semibold text-gray-900">
                  {{ dashboard.inProgressTasks }}
                </p>
              </div>

              <div class="flex h-10 w-10 items-center justify-center rounded-xl bg-blue-50">
                <Clock3 class="h-5 w-5 text-blue-600" />
              </div>
            </div>
          </div>

          <!-- Completed -->
          <div class="rounded-2xl border border-gray-100 bg-white p-5 shadow-sm">
            <div class="flex items-start justify-between gap-3">
              <div>
                <p class="text-sm font-medium text-gray-500">Completed</p>

                <p class="mt-2 text-3xl font-semibold text-gray-900">
                  {{ dashboard.completedTasks }}
                </p>
              </div>

              <div class="flex h-10 w-10 items-center justify-center rounded-xl bg-green-50">
                <CheckCircle2 class="h-5 w-5 text-green-600" />
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Admin task overview -->
      <section v-if="isAdmin">
        <div class="rounded-2xl border border-gray-100 bg-white p-5 shadow-sm">
          <div>
            <h2 class="text-base font-semibold text-gray-900">Task Overview</h2>

            <p class="mt-1 text-sm text-gray-500">Current task distribution across the system.</p>
          </div>

          <div class="mt-5 space-y-4">
            <!-- Pending -->
            <div>
              <div class="mb-1.5 flex justify-between text-sm">
                <span class="text-gray-600"> Pending </span>

                <span class="font-medium text-gray-900">
                  {{ dashboard.pendingTasks }}
                </span>
              </div>

              <div class="h-2 overflow-hidden rounded-full bg-gray-100">
                <div
                  class="h-full rounded-full bg-gray-400"
                  :style="{
                    width:
                      dashboard.totalTasks > 0
                        ? `${(dashboard.pendingTasks / dashboard.totalTasks) * 100}%`
                        : '0%',
                  }"
                ></div>
              </div>
            </div>

            <!-- In Progress -->
            <div>
              <div class="mb-1.5 flex justify-between text-sm">
                <span class="text-gray-600"> In Progress </span>

                <span class="font-medium text-gray-900">
                  {{ dashboard.inProgressTasks }}
                </span>
              </div>

              <div class="h-2 overflow-hidden rounded-full bg-gray-100">
                <div
                  class="h-full rounded-full bg-blue-500"
                  :style="{
                    width:
                      dashboard.totalTasks > 0
                        ? `${(dashboard.inProgressTasks / dashboard.totalTasks) * 100}%`
                        : '0%',
                  }"
                ></div>
              </div>
            </div>

            <!-- Completed -->
            <div>
              <div class="mb-1.5 flex justify-between text-sm">
                <span class="text-gray-600"> Completed </span>

                <span class="font-medium text-gray-900">
                  {{ dashboard.completedTasks }}
                </span>
              </div>

              <div class="h-2 overflow-hidden rounded-full bg-gray-100">
                <div
                  class="h-full rounded-full bg-green-500"
                  :style="{
                    width:
                      dashboard.totalTasks > 0
                        ? `${(dashboard.completedTasks / dashboard.totalTasks) * 100}%`
                        : '0%',
                  }"
                ></div>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- High priority -->
      <section>
        <div class="rounded-2xl border border-blue-100 bg-blue-50/50 p-5 shadow-sm">
          <div class="flex items-center justify-between gap-4">
            <div>
              <p class="text-sm font-medium text-blue-700">High Priority Tasks</p>

              <p class="mt-1 text-sm text-blue-600/70">
                {{
                  isAdmin
                    ? 'High priority tasks across the system'
                    : 'Tasks that need your attention'
                }}
              </p>
            </div>

            <div
              class="flex h-12 w-12 shrink-0 items-center justify-center rounded-xl bg-white shadow-sm"
            >
              <AlertTriangle class="h-5 w-5 text-blue-600" />
            </div>
          </div>

          <p class="mt-4 text-3xl font-semibold text-gray-900">
            {{ dashboard.highPriorityTasks }}
          </p>
        </div>
      </section>
    </template>
  </div>
</template>
```
