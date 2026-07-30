<script setup lang="ts">
import type { User } from '@/types/user'

defineProps<{
  users: User[]
}>()
</script>

<template>
  <div class="bg-white rounded shadow">
    <table class="w-full">
      <thead>
        <tr class="border-b">
          <th class="text-left p-4">Name</th>

          <th class="text-left p-4">Email</th>

          <th class="text-left p-4">Role</th>

          <th class="text-left p-4">Created</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="user in users" :key="user.id" class="border-b hover:bg-gray-50">
          <td class="p-4">
            {{ user.name }}
          </td>

          <td class="p-4">
            {{ user.email }}
          </td>

          <td class="p-4">
            {{ user.role }}
          </td>

          <td class="p-4">
            {{ new Date(user.createdDate).toLocaleDateString() }}
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
