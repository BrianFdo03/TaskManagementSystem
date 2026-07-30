<script setup lang="ts">
import { onMounted, ref } from 'vue'

import userService from '@/services/userService'

import type { User } from '@/types/user'

import UserList from '@/components/users/UserList.vue'

const users = ref<User[]>([])

const loading = ref(false)

const error = ref('')

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

onMounted(() => {
  loadUsers()
})
</script>

<template>
  <div>
    <h1 class="text-2xl font-bold mb-6">Users</h1>

    <div v-if="loading">Loading users...</div>

    <div v-else-if="error" class="text-red-500">
      {{ error }}
    </div>

    <UserList v-else :users="users" />
  </div>
</template>
