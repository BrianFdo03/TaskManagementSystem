<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { Plus, Search, RefreshCw, UsersRound } from 'lucide-vue-next'

import userService from '@/services/userService'

import type { CreateUserRequest, UpdateUserRequest, User } from '@/types/user'

import UserList from '@/components/users/UserList.vue'
import UserFormModal from '@/components/users/UserFormModal.vue'
import UserDeleteModal from '@/components/users/UserDeleteModal.vue'

const users = ref<User[]>([])

const loading = ref(false)
const error = ref('')
const searchQuery = ref('')

const showUserModal = ref(false)
const modalLoading = ref(false)
const modalError = ref('')

const editingUser = ref<User | null>(null)

const showDeleteModal = ref(false)
const deleteLoading = ref(false)
const deleteError = ref('')

const deletingUser = ref<User | null>(null)

const loadUsers = async () => {
  loading.value = true
  error.value = ''

  try {
    users.value = await userService.getUsers()
  } catch (err) {
    error.value = 'Failed to load users.'
    console.error(err)
  } finally {
    loading.value = false
  }
}

const filteredUsers = computed(() => {
  const query = searchQuery.value.trim().toLowerCase()

  if (!query) {
    return users.value
  }

  return users.value.filter((user) => {
    return (
      user.name.toLowerCase().includes(query) ||
      user.email.toLowerCase().includes(query) ||
      user.role.toLowerCase().includes(query)
    )
  })
})

const openCreateModal = () => {
  editingUser.value = null
  modalError.value = ''
  showUserModal.value = true
}

const openEditModal = (user: User) => {
  editingUser.value = user
  modalError.value = ''
  showUserModal.value = true
}

const closeUserModal = () => {
  if (modalLoading.value) {
    return
  }

  showUserModal.value = false
  editingUser.value = null
  modalError.value = ''
}

const handleUserSubmit = async (data: CreateUserRequest | UpdateUserRequest) => {
  modalLoading.value = true
  modalError.value = ''

  try {
    if (editingUser.value) {
      await userService.updateUser(editingUser.value.id, data as UpdateUserRequest)
    } else {
      await userService.createUser(data as CreateUserRequest)
    }

    await loadUsers()

    showUserModal.value = false
    editingUser.value = null
    modalError.value = ''
  } catch (err: any) {
    console.error(err)

    if (err.response?.status === 400) {
      modalError.value = err.response?.data?.message ?? 'The submitted information is invalid.'
    } else if (err.response?.status === 409) {
      modalError.value = err.response?.data?.message ?? 'A user with this email already exists.'
    } else {
      modalError.value = editingUser.value ? 'Failed to update user.' : 'Failed to create user.'
    }
  } finally {
    modalLoading.value = false
  }
}

const openDeleteModal = (user: User) => {
  deletingUser.value = user
  deleteError.value = ''
  showDeleteModal.value = true
}

const closeDeleteModal = () => {
  if (deleteLoading.value) {
    return
  }

  showDeleteModal.value = false
  deletingUser.value = null
  deleteError.value = ''
}

const handleDelete = (user: User) => {
  openDeleteModal(user)
}

const confirmDelete = async () => {
  if (!deletingUser.value) {
    return
  }

  deleteLoading.value = true
  deleteError.value = ''

  try {
    await userService.deleteUser(deletingUser.value.id)

    showDeleteModal.value = false
    deletingUser.value = null
    deleteError.value = ''

    await loadUsers()
  } catch (err: any) {
    console.error(err)

    if (err.response?.status === 404) {
      deleteError.value = 'The user could not be found. It may have already been deleted.'
    } else if (err.response?.status === 409) {
      deleteError.value =
        err.response?.data?.message ??
        'This user cannot be deleted because they have related tasks.'
    } else {
      deleteError.value = 'Failed to delete the user. Please try again.'
    }
  } finally {
    deleteLoading.value = false
  }
}
onMounted(() => {
  loadUsers()
})
</script>

<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
      <div>
        <div class="flex items-center gap-2">
          <UsersRound :size="24" class="text-blue-600" />

          <h1 class="text-2xl font-semibold text-gray-900">Users</h1>
        </div>

        <p class="mt-1 text-sm text-gray-500">Manage users in the system.</p>
      </div>

      <button
        type="button"
        @click="openCreateModal"
        class="inline-flex items-center justify-center gap-2 rounded-lg bg-blue-600 px-4 py-2.5 text-sm font-medium text-white transition hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
      >
        <Plus :size="18" />
        Add User
      </button>
    </div>

    <!-- Search -->
    <div class="relative">
      <Search
        :size="19"
        class="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-gray-400"
      />

      <input
        v-model="searchQuery"
        type="search"
        placeholder="Search users..."
        class="w-full rounded-xl border border-gray-200 bg-white py-3 pl-10 pr-4 text-sm text-gray-900 outline-none transition placeholder:text-gray-400 focus:border-blue-500 focus:ring-2 focus:ring-blue-500/20"
      />
    </div>

    <!-- Count / Refresh -->
    <div class="flex items-center justify-between">
      <p class="text-sm text-gray-500">
        {{ filteredUsers.length }}
        {{ filteredUsers.length === 1 ? 'user' : 'users' }}
      </p>

      <button
        type="button"
        @click="loadUsers"
        :disabled="loading"
        class="inline-flex items-center gap-1.5 rounded-lg px-3 py-2 text-sm text-gray-500 transition hover:bg-gray-100 hover:text-gray-900 disabled:cursor-not-allowed disabled:opacity-50"
      >
        <RefreshCw :size="16" :class="{ 'animate-spin': loading }" />

        Refresh
      </button>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="rounded-xl border border-gray-200 bg-white p-10 text-center">
      <RefreshCw :size="24" class="mx-auto animate-spin text-blue-600" />

      <p class="mt-3 text-sm text-gray-500">Loading users...</p>
    </div>

    <!-- Error -->
    <div v-else-if="error" class="rounded-xl border border-red-200 bg-red-50 p-5">
      <p class="text-sm font-medium text-red-700">
        {{ error }}
      </p>

      <button
        type="button"
        @click="loadUsers"
        class="mt-3 text-sm font-medium text-red-700 underline hover:no-underline"
      >
        Try again
      </button>
    </div>

    <!-- User List -->
    <UserList v-else :users="filteredUsers" @edit="openEditModal" @delete="handleDelete" />

    <!-- User Form Modal -->
    <UserFormModal
      :open="showUserModal"
      :user="editingUser"
      :loading="modalLoading"
      :error="modalError"
      @close="closeUserModal"
      @submit="handleUserSubmit"
    />

    <!-- Delete Confirmation -->
    <UserDeleteModal
      :open="showDeleteModal"
      :user="deletingUser"
      :loading="deleteLoading"
      :error="deleteError"
      @close="closeDeleteModal"
      @confirm="confirmDelete"
    />
  </div>
</template>
