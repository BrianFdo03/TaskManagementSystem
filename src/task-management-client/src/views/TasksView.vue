<script setup lang="ts">
import { onMounted, ref, watch } from 'vue'
import TaskList from '@/components/tasks/TaskList.vue'
import taskService from '@/services/taskService'
import type { Task } from '@/types/task'
import { useUserSelectionStore } from '@/stores/userSelection'
import { useAuthStore } from '@/stores/auth'

const userSelectionStore = useUserSelectionStore()
const authStore = useAuthStore()
const tasks = ref<Task[]>([])
const loading = ref(false)
const error = ref('')

const isAdmin = () => {
  return authStore.user?.role === 'Admin'
}

const loadTasks = async () => {
  tasks.value = []

  error.value = ''

  /* Admins must select a user before loading tasks. */
  if (isAdmin() && !userSelectionStore.selectedUser) {
    return
  }

  loading.value = true

  try {
    if (isAdmin() && userSelectionStore.selectedUser) {
      tasks.value = await taskService.getTasksByUser(userSelectionStore.selectedUser.id)
    } else {
      tasks.value = await taskService.getMyTasks()
    }
  } catch (err) {
    console.error(err)

    error.value = 'Unable to load tasks.'
  } finally {
    loading.value = false
  }
}

onMounted(loadTasks)

watch(
  () => userSelectionStore.selectedUser?.id,

  () => {
    loadTasks()
  },
)
</script>

<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold">Tasks</h1>
    </div>

    <!-- Admin with no selected user -->

    <div v-if="isAdmin() && !userSelectionStore.selectedUser" class="bg-white p-6 rounded shadow">
      <p class="text-gray-600">Please select a user to view their tasks.</p>
    </div>

    <!-- Loading -->

    <div v-else-if="loading" class="text-gray-600">Loading tasks...</div>

    <!-- Error -->

    <div v-else-if="error" class="text-red-500">
      {{ error }}
    </div>

    <!-- Tasks -->

    <div v-else>
      <div v-if="userSelectionStore.selectedUser" class="mb-4">
        <p class="text-gray-600">
          Showing tasks for:

          <span class="font-semibold">
            {{ userSelectionStore.selectedUser.name }}
          </span>
        </p>
      </div>

      <TaskList v-else :tasks="tasks" />
    </div>
  </div>
</template>
