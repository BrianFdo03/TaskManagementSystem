<script setup lang="ts">
import { ref } from 'vue'
import { useRouter, RouterLink, RouterView } from 'vue-router'
import { LayoutDashboard, Users, ListTodo, LogOut, Menu, X, UserCircle } from 'lucide-vue-next'

import { openNativeAboutPage } from '@/utils/nativeNavigation'

import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()
const router = useRouter()

const mobileMenuOpen = ref(false)

const logout = async () => {
  authStore.logout()
  mobileMenuOpen.value = false

  await router.push('/login')
}
</script>

<template>
  <div class="min-h-screen bg-white text-gray-900">
    <!-- Desktop Sidebar -->
    <aside
      class="fixed inset-y-0 left-0 z-40 hidden w-64 border-r border-gray-200 bg-white lg:flex lg:flex-col"
    >
      <!-- Logo -->
      <div class="flex h-16 items-center border-b border-gray-200 px-6">
        <div class="flex items-center gap-3">
          <div class="flex h-9 w-9 items-center justify-center rounded-lg bg-blue-600 text-white">
            <ListTodo :size="20" />
          </div>

          <span class="text-lg font-semibold"> Task Management </span>
        </div>
      </div>

      <!-- Navigation -->
      <nav class="flex-1 space-y-1 px-3 py-5">
        <RouterLink
          to="/dashboard"
          class="flex items-center gap-3 rounded-lg px-3 py-2.5 text-sm font-medium text-gray-600 transition hover:bg-blue-50 hover:text-blue-600"
          active-class="bg-blue-50 text-blue-600"
        >
          <LayoutDashboard :size="20" />
          Dashboard
        </RouterLink>

        <RouterLink
          v-if="authStore.user?.role === 'Admin'"
          to="/users"
          class="flex items-center gap-3 rounded-lg px-3 py-2.5 text-sm font-medium text-gray-600 transition hover:bg-blue-50 hover:text-blue-600"
          active-class="bg-blue-50 text-blue-600"
        >
          <Users :size="20" />
          Users
        </RouterLink>

        <RouterLink
          to="/tasks"
          class="flex items-center gap-3 rounded-lg px-3 py-2.5 text-sm font-medium text-gray-600 transition hover:bg-blue-50 hover:text-blue-600"
          active-class="bg-blue-50 text-blue-600"
        >
          <ListTodo :size="20" />
          Tasks
        </RouterLink>
        <button
          type="button"
          @click="openNativeAboutPage"
          class="flex w-full items-center gap-3 rounded-lg px-3 py-2.5 text-sm font-medium text-gray-600 transition hover:bg-blue-50 hover:text-blue-600"
        >
          <Info :size="20" />
          About App
        </button>
      </nav>

      <!-- User / Logout -->
      <div class="border-t border-gray-200 p-4">
        <div class="mb-3 flex items-center gap-3 px-2">
          <UserCircle :size="32" class="text-gray-400" />

          <div class="min-w-0">
            <p class="truncate text-sm font-medium">
              {{ authStore.user?.name }}
            </p>

            <p class="truncate text-xs text-gray-500">
              {{ authStore.user?.role }}
            </p>
          </div>
        </div>

        <button
          type="button"
          @click="logout"
          class="flex w-full items-center gap-3 rounded-lg px-3 py-2.5 text-sm font-medium text-gray-600 transition hover:bg-gray-100 hover:text-gray-900"
        >
          <LogOut :size="20" />
          Logout
        </button>
      </div>
    </aside>

    <!-- Mobile Header -->
    <header
      class="sticky top-0 z-30 flex h-16 items-center justify-between border-b border-gray-200 bg-white px-4 lg:hidden"
    >
      <button
        type="button"
        @click="mobileMenuOpen = true"
        class="rounded-lg p-2 text-gray-600 hover:bg-gray-100"
        aria-label="Open menu"
      >
        <Menu :size="22" />
      </button>

      <div class="flex items-center gap-2">
        <div class="flex h-8 w-8 items-center justify-center rounded-lg bg-blue-600 text-white">
          <ListTodo :size="18" />
        </div>

        <span class="text-sm font-semibold"> Task Management </span>
      </div>

      <UserCircle :size="24" class="text-gray-400" />
    </header>

    <!-- Mobile Menu Overlay -->
    <Transition name="fade">
      <div v-if="mobileMenuOpen" class="fixed inset-0 z-50 lg:hidden">
        <div class="absolute inset-0 bg-black/20" @click="mobileMenuOpen = false" />

        <aside class="absolute inset-y-0 left-0 flex w-72 flex-col bg-white shadow-xl">
          <div class="flex h-16 items-center justify-between border-b border-gray-200 px-5">
            <div class="flex items-center gap-3">
              <div
                class="flex h-9 w-9 items-center justify-center rounded-lg bg-blue-600 text-white"
              >
                <ListTodo :size="20" />
              </div>

              <span class="font-semibold"> Task Management </span>
            </div>

            <button
              type="button"
              @click="mobileMenuOpen = false"
              class="rounded-lg p-2 text-gray-500 hover:bg-gray-100"
            >
              <X :size="20" />
            </button>
          </div>

          <nav class="flex-1 space-y-1 px-3 py-5">
            <RouterLink
              to="/dashboard"
              @click="mobileMenuOpen = false"
              class="flex items-center gap-3 rounded-lg px-3 py-3 text-sm font-medium text-gray-600 hover:bg-blue-50 hover:text-blue-600"
              active-class="bg-blue-50 text-blue-600"
            >
              <LayoutDashboard :size="20" />
              Dashboard
            </RouterLink>

            <RouterLink
              v-if="authStore.user?.role === 'Admin'"
              to="/users"
              @click="mobileMenuOpen = false"
              class="flex items-center gap-3 rounded-lg px-3 py-3 text-sm font-medium text-gray-600 hover:bg-blue-50 hover:text-blue-600"
              active-class="bg-blue-50 text-blue-600"
            >
              <Users :size="20" />
              Users
            </RouterLink>

            <RouterLink
              to="/tasks"
              @click="mobileMenuOpen = false"
              class="flex items-center gap-3 rounded-lg px-3 py-3 text-sm font-medium text-gray-600 hover:bg-blue-50 hover:text-blue-600"
              active-class="bg-blue-50 text-blue-600"
            >
              <ListTodo :size="20" />
              Tasks
            </RouterLink>
            <button
              type="button"
              @click="openNativeAboutPage"
              class="flex w-full items-center gap-3 rounded-lg px-3 py-3 text-sm font-medium text-gray-600 hover:bg-blue-50 hover:text-blue-600"
            >
              <Info :size="20" />
              About App
            </button>
          </nav>

          <div class="border-t border-gray-200 p-4">
            <div class="mb-3 flex items-center gap-3 px-2">
              <UserCircle :size="32" class="text-gray-400" />

              <div class="min-w-0">
                <p class="truncate text-sm font-medium">
                  {{ authStore.user?.name }}
                </p>

                <p class="truncate text-xs text-gray-500">
                  {{ authStore.user?.role }}
                </p>
              </div>
            </div>

            <button
              type="button"
              @click="logout"
              class="flex w-full items-center gap-3 rounded-lg px-3 py-3 text-sm font-medium text-gray-600 hover:bg-gray-100"
            >
              <LogOut :size="20" />
              Logout
            </button>
          </div>
        </aside>
      </div>
    </Transition>

    <!-- Main Content -->
    <main class="min-h-screen lg:pl-64">
      <div class="mx-auto w-full max-w-7xl px-4 py-6 pb-24 sm:px-6 lg:px-8 lg:py-8 lg:pb-8">
        <RouterView />
      </div>
    </main>

    <!-- Mobile Bottom Navigation -->
    <nav class="fixed inset-x-0 bottom-0 z-30 border-t border-gray-200 bg-white lg:hidden">
      <div
        :class="['grid h-[68px]', authStore.user?.role === 'Admin' ? 'grid-cols-3' : 'grid-cols-2']"
      >
        <!-- Dashboard -->
        <RouterLink
          to="/dashboard"
          class="flex flex-col items-center justify-center gap-1 text-xs text-gray-500 transition-colors"
          active-class="text-blue-600"
        >
          <LayoutDashboard :size="20" />
          <span>Dashboard</span>
        </RouterLink>

        <!-- Users - Admin only -->
        <RouterLink
          v-if="authStore.user?.role === 'Admin'"
          to="/users"
          class="flex flex-col items-center justify-center gap-1 text-xs text-gray-500 transition-colors"
          active-class="text-blue-600"
        >
          <Users :size="20" />
          <span>Users</span>
        </RouterLink>

        <!-- Tasks -->
        <RouterLink
          to="/tasks"
          class="flex flex-col items-center justify-center gap-1 text-xs text-gray-500 transition-colors"
          active-class="text-blue-600"
        >
          <ListTodo :size="20" />
          <span>Tasks</span>
        </RouterLink>
      </div>
    </nav>
  </div>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
