<script setup lang="ts">
import { ref, watch } from 'vue'
import { AlertTriangle, Loader2, Trash2, X } from 'lucide-vue-next'

import type { User } from '@/types/user'

const props = defineProps<{
  open: boolean
  user: User | null
  loading?: boolean
  error?: string
}>()

const emit = defineEmits<{
  close: []
  confirm: []
}>()

const visible = ref(false)

watch(
  () => props.open,
  (open) => {
    visible.value = open
  },
  { immediate: true },
)
</script>

<template>
  <Transition name="modal">
    <div
      v-if="open"
      class="fixed inset-0 z-50 flex items-end justify-center bg-black/40 p-0 sm:items-center sm:p-4"
      @click.self="emit('close')"
    >
      <div class="w-full max-w-md rounded-t-2xl bg-white shadow-xl sm:rounded-2xl">
        <!-- Header -->
        <div class="flex items-center justify-between border-b border-gray-100 px-5 py-4">
          <div class="flex items-center gap-3">
            <div
              class="flex h-10 w-10 items-center justify-center rounded-full bg-red-50 text-red-600"
            >
              <AlertTriangle :size="20" />
            </div>

            <div>
              <h2 class="text-base font-semibold text-gray-900">Delete User</h2>

              <p class="text-xs text-gray-500">This action cannot be undone.</p>
            </div>
          </div>

          <button
            type="button"
            @click="emit('close')"
            :disabled="loading"
            class="rounded-lg p-2 text-gray-400 transition hover:bg-gray-100 hover:text-gray-700 disabled:opacity-50"
          >
            <X :size="20" />
          </button>
        </div>

        <!-- Content -->
        <div class="px-5 py-5">
          <p class="text-sm leading-6 text-gray-600">
            Are you sure you want to delete
            <span class="font-semibold text-gray-900">
              {{ user?.name }}
            </span>
            ? This will permanently remove the user from the system.
          </p>

          <!-- API Error -->
          <div
            v-if="error"
            class="mt-4 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700"
          >
            {{ error }}
          </div>

          <!-- Actions -->
          <div class="mt-6 flex flex-col-reverse gap-2 sm:flex-row sm:justify-end">
            <button
              type="button"
              @click="emit('close')"
              :disabled="loading"
              class="rounded-lg border border-gray-200 px-4 py-2.5 text-sm font-medium text-gray-700 transition hover:bg-gray-50 disabled:opacity-50"
            >
              Cancel
            </button>

            <button
              type="button"
              @click="emit('confirm')"
              :disabled="loading"
              class="inline-flex items-center justify-center gap-2 rounded-lg bg-red-600 px-4 py-2.5 text-sm font-medium text-white transition hover:bg-red-700 disabled:cursor-not-allowed disabled:opacity-60"
            >
              <Loader2 v-if="loading" :size="17" class="animate-spin" />

              <Trash2 v-else :size="17" />

              {{ loading ? 'Deleting...' : 'Delete User' }}
            </button>
          </div>
        </div>
      </div>
    </div>
  </Transition>
</template>

<style scoped>
.modal-enter-active,
.modal-leave-active {
  transition: opacity 0.2s ease;
}

.modal-enter-active > div,
.modal-leave-active > div {
  transition: transform 0.2s ease;
}

.modal-enter-from,
.modal-leave-to {
  opacity: 0;
}

.modal-enter-from > div,
.modal-leave-to > div {
  transform: translateY(20px);
}
</style>
