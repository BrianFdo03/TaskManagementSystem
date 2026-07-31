<script setup lang="ts">
import { Mail, Pencil, Trash2, UserCircle } from 'lucide-vue-next'

import type { User } from '@/types/user'

defineProps<{
  users: User[]
}>()

const emit = defineEmits<{
  edit: [user: User]
  delete: [user: User]
}>()

const formatDate = (date: string) => {
  return new Date(date).toLocaleDateString()
}

const getRoleClasses = (role: string) => {
  if (role === 'Admin') {
    return 'bg-blue-50 text-blue-700 ring-blue-600/20'
  }

  return 'bg-gray-100 text-gray-700 ring-gray-500/20'
}
</script>

<template>
  <!-- Empty state -->
  <div
    v-if="users.length === 0"
    class="rounded-xl border border-gray-200 bg-white p-10 text-center"
  >
    <UserCircle :size="40" class="mx-auto text-gray-300" />

    <h3 class="mt-4 text-sm font-semibold text-gray-900">No users found</h3>

    <p class="mt-1 text-sm text-gray-500">Try changing your search.</p>
  </div>

  <template v-else>
    <!-- Desktop / Tablet -->
    <div class="hidden overflow-hidden rounded-xl border border-gray-200 bg-white md:block">
      <div class="overflow-x-auto">
        <table class="w-full">
          <thead>
            <tr class="border-b border-gray-200 bg-gray-50/70">
              <th
                class="px-5 py-3.5 text-left text-xs font-semibold uppercase tracking-wide text-gray-500"
              >
                User
              </th>

              <th
                class="px-5 py-3.5 text-left text-xs font-semibold uppercase tracking-wide text-gray-500"
              >
                Email
              </th>

              <th
                class="px-5 py-3.5 text-left text-xs font-semibold uppercase tracking-wide text-gray-500"
              >
                Role
              </th>

              <th
                class="px-5 py-3.5 text-left text-xs font-semibold uppercase tracking-wide text-gray-500"
              >
                Created
              </th>

              <th
                class="px-5 py-3.5 text-right text-xs font-semibold uppercase tracking-wide text-gray-500"
              >
                Actions
              </th>
            </tr>
          </thead>

          <tbody class="divide-y divide-gray-100">
            <tr v-for="user in users" :key="user.id" class="transition hover:bg-gray-50">
              <!-- User -->
              <td class="px-5 py-4">
                <div class="flex items-center gap-3">
                  <div
                    class="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-blue-50 text-blue-600"
                  >
                    <UserCircle :size="20" />
                  </div>

                  <span class="text-sm font-medium text-gray-900">
                    {{ user.name }}
                  </span>
                </div>
              </td>

              <!-- Email -->
              <td class="px-5 py-4">
                <div class="flex items-center gap-2 text-sm text-gray-500">
                  <Mail :size="16" />
                  {{ user.email }}
                </div>
              </td>

              <!-- Role -->
              <td class="px-5 py-4">
                <span
                  class="inline-flex rounded-full px-2.5 py-1 text-xs font-medium ring-1 ring-inset"
                  :class="getRoleClasses(user.role)"
                >
                  {{ user.role }}
                </span>
              </td>

              <!-- Created -->
              <td class="px-5 py-4 text-sm text-gray-500">
                {{ formatDate(user.createdDate) }}
              </td>

              <!-- Actions -->
              <td class="px-5 py-4">
                <div class="flex justify-end gap-1">
                  <button
                    type="button"
                    @click="emit('edit', user)"
                    class="rounded-lg p-2 text-gray-400 transition hover:bg-blue-50 hover:text-blue-600"
                    title="Edit user"
                  >
                    <Pencil :size="17" />
                  </button>

                  <button
                    type="button"
                    @click="emit('delete', user)"
                    class="rounded-lg p-2 text-gray-400 transition hover:bg-red-50 hover:text-red-600"
                    title="Delete user"
                  >
                    <Trash2 :size="17" />
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Mobile -->
    <div class="space-y-3 md:hidden">
      <div
        v-for="user in users"
        :key="user.id"
        class="rounded-xl border border-gray-200 bg-white p-4"
      >
        <div class="flex items-start justify-between gap-3">
          <div class="flex min-w-0 items-center gap-3">
            <div
              class="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-blue-50 text-blue-600"
            >
              <UserCircle :size="22" />
            </div>

            <div class="min-w-0">
              <p class="truncate text-sm font-semibold text-gray-900">
                {{ user.name }}
              </p>

              <p class="mt-0.5 truncate text-xs text-gray-500">
                {{ user.email }}
              </p>
            </div>
          </div>

          <!-- Mobile actions -->
          <div class="flex shrink-0 gap-1">
            <button
              type="button"
              @click="emit('edit', user)"
              class="rounded-lg p-2 text-gray-400 transition hover:bg-blue-50 hover:text-blue-600"
              title="Edit user"
            >
              <Pencil :size="17" />
            </button>

            <button
              type="button"
              @click="emit('delete', user)"
              class="rounded-lg p-2 text-gray-400 transition hover:bg-red-50 hover:text-red-600"
              title="Delete user"
            >
              <Trash2 :size="17" />
            </button>
          </div>
        </div>

        <div class="mt-4 flex items-center justify-between">
          <span
            class="inline-flex rounded-full px-2.5 py-1 text-xs font-medium ring-1 ring-inset"
            :class="getRoleClasses(user.role)"
          >
            {{ user.role }}
          </span>

          <span class="text-xs text-gray-400">
            {{ formatDate(user.createdDate) }}
          </span>
        </div>
      </div>
    </div>
  </template>
</template>
