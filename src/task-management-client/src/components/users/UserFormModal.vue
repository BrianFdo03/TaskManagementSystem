<script setup lang="ts">
import { computed, reactive, ref, watch } from 'vue'
import { X, UserPlus, Save, Loader2 } from 'lucide-vue-next'

import type { CreateUserRequest, UpdateUserRequest, User } from '@/types/user'

const props = defineProps<{
  open: boolean
  user?: User | null
  loading?: boolean
  error?: string
}>()

const emit = defineEmits<{
  close: []
  submit: [data: CreateUserRequest | UpdateUserRequest]
}>()

const isEditMode = computed(() => !!props.user)

const form = reactive({
  name: '',
  email: '',
  password: '',
  role: 2,
})

const errors = reactive({
  name: '',
  email: '',
  password: '',
  role: '',
})

const resetErrors = () => {
  errors.name = ''
  errors.email = ''
  errors.password = ''
  errors.role = ''
}

const resetForm = () => {
  form.name = ''
  form.email = ''
  form.password = ''
  form.role = 2

  resetErrors()
}

const populateForm = () => {
  resetErrors()

  if (props.user) {
    form.name = props.user.name
    form.email = props.user.email

    form.role = props.user.role === 'Admin' ? 1 : 2

    form.password = ''
  } else {
    resetForm()
  }
}

watch(
  () => props.open,
  (open) => {
    if (open) {
      populateForm()
    }
  },
)

watch(
  () => props.user,
  () => {
    if (props.open) {
      populateForm()
    }
  },
)

const validate = () => {
  resetErrors()

  let valid = true

  if (!form.name.trim()) {
    errors.name = 'Name is required.'
    valid = false
  }

  if (!form.email.trim()) {
    errors.email = 'Email is required.'
    valid = false
  } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email)) {
    errors.email = 'Enter a valid email address.'
    valid = false
  }

  if (!isEditMode.value && !form.password) {
    errors.password = 'Password is required.'
    valid = false
  }

  if (!isEditMode.value && form.password.length < 6) {
    errors.password = 'Password must be at least 6 characters.'
    valid = false
  }

  if (![1, 2].includes(form.role)) {
    errors.role = 'Select a valid role.'
    valid = false
  }

  return valid
}

const handleSubmit = () => {
  if (!validate()) {
    return
  }

  if (isEditMode.value) {
    const request: UpdateUserRequest = {
      name: form.name.trim(),
      email: form.email.trim(),
      role: form.role,
    }

    emit('submit', request)
    return
  }

  const request: CreateUserRequest = {
    name: form.name.trim(),
    email: form.email.trim(),
    password: form.password,
    role: form.role,
  }

  emit('submit', request)
}
</script>

<template>
  <!-- Backdrop -->
  <Transition name="modal">
    <div
      v-if="open"
      class="fixed inset-0 z-50 flex items-end justify-center bg-black/40 p-0 sm:items-center sm:p-4"
      @click.self="emit('close')"
    >
      <!-- Modal -->
      <div class="w-full max-w-lg rounded-t-2xl bg-white shadow-xl sm:rounded-2xl">
        <!-- Header -->
        <div class="flex items-center justify-between border-b border-gray-100 px-5 py-4">
          <div class="flex items-center gap-3">
            <div
              class="flex h-10 w-10 items-center justify-center rounded-full bg-blue-50 text-blue-600"
            >
              <UserPlus v-if="!isEditMode" :size="20" />

              <Save v-else :size="20" />
            </div>

            <div>
              <h2 class="text-base font-semibold text-gray-900">
                {{ isEditMode ? 'Edit User' : 'Add User' }}
              </h2>

              <p class="text-xs text-gray-500">
                {{ isEditMode ? 'Update user information.' : 'Create a new user account.' }}
              </p>
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

        <!-- Form -->
        <form @submit.prevent="handleSubmit" class="space-y-5 px-5 py-5">
          <!-- API error -->
          <div
            v-if="error"
            class="rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700"
          >
            {{ error }}
          </div>

          <!-- Name -->
          <div>
            <label for="user-name" class="mb-1.5 block text-sm font-medium text-gray-700">
              Name
            </label>

            <input
              id="user-name"
              v-model="form.name"
              type="text"
              placeholder="Enter user name"
              :disabled="loading"
              class="w-full rounded-lg border border-gray-200 px-3.5 py-2.5 text-sm outline-none transition placeholder:text-gray-400 focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20 disabled:bg-gray-50"
              :class="{
                'border-red-400 focus:border-red-500 focus:ring-red-500/20': errors.name,
              }"
            />

            <p v-if="errors.name" class="mt-1 text-xs text-red-600">
              {{ errors.name }}
            </p>
          </div>

          <!-- Email -->
          <div>
            <label for="user-email" class="mb-1.5 block text-sm font-medium text-gray-700">
              Email
            </label>

            <input
              id="user-email"
              v-model="form.email"
              type="email"
              placeholder="user@example.com"
              :disabled="loading"
              class="w-full rounded-lg border border-gray-200 px-3.5 py-2.5 text-sm outline-none transition placeholder:text-gray-400 focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20 disabled:bg-gray-50"
              :class="{
                'border-red-400 focus:border-red-500 focus:ring-red-500/20': errors.email,
              }"
            />

            <p v-if="errors.email" class="mt-1 text-xs text-red-600">
              {{ errors.email }}
            </p>
          </div>

          <!-- Password -->
          <div v-if="!isEditMode">
            <label for="user-password" class="mb-1.5 block text-sm font-medium text-gray-700">
              Password
            </label>

            <input
              id="user-password"
              v-model="form.password"
              type="password"
              placeholder="Enter password"
              :disabled="loading"
              class="w-full rounded-lg border border-gray-200 px-3.5 py-2.5 text-sm outline-none transition placeholder:text-gray-400 focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20 disabled:bg-gray-50"
              :class="{
                'border-red-400 focus:border-red-500 focus:ring-red-500/20': errors.password,
              }"
            />

            <p v-if="errors.password" class="mt-1 text-xs text-red-600">
              {{ errors.password }}
            </p>
          </div>

          <!-- Role -->
          <div>
            <label for="user-role" class="mb-1.5 block text-sm font-medium text-gray-700">
              Role
            </label>

            <select
              id="user-role"
              v-model="form.role"
              :disabled="loading"
              class="w-full rounded-lg border border-gray-200 bg-white px-3.5 py-2.5 text-sm outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20 disabled:bg-gray-50"
              :class="{
                'border-red-400 focus:border-red-500 focus:ring-red-500/20': errors.role,
              }"
            >
              <option :value="2">Employee</option>

              <option :value="1">Admin</option>
            </select>

            <p v-if="errors.role" class="mt-1 text-xs text-red-600">
              {{ errors.role }}
            </p>
          </div>

          <!-- Actions -->
          <div class="flex flex-col-reverse gap-2 pt-2 sm:flex-row sm:justify-end">
            <button
              type="button"
              @click="emit('close')"
              :disabled="loading"
              class="rounded-lg border border-gray-200 px-4 py-2.5 text-sm font-medium text-gray-700 transition hover:bg-gray-50 disabled:opacity-50"
            >
              Cancel
            </button>

            <button
              type="submit"
              :disabled="loading"
              class="inline-flex items-center justify-center gap-2 rounded-lg bg-blue-600 px-4 py-2.5 text-sm font-medium text-white transition hover:bg-blue-700 disabled:cursor-not-allowed disabled:opacity-60"
            >
              <Loader2 v-if="loading" :size="17" class="animate-spin" />

              <UserPlus v-else-if="!isEditMode" :size="17" />

              <Save v-else :size="17" />

              {{ loading ? 'Saving...' : isEditMode ? 'Save Changes' : 'Create User' }}
            </button>
          </div>
        </form>
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
