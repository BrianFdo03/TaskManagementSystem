<script setup lang="ts">
import { onMounted, ref, watch } from 'vue'
import TaskList from '@/components/tasks/TaskList.vue'
import TaskForm from '@/components/tasks/TaskForm.vue'
import taskService from '@/services/taskService'
import type { Task } from '@/types/task'
import { useUserSelectionStore } from '@/stores/userSelection'
import { useAuthStore } from '@/stores/auth'

const userSelectionStore = useUserSelectionStore()
const authStore = useAuthStore()
const tasks = ref<Task[]>([])
const loading = ref(false)
const error = ref('')
const showTaskForm = ref(false)

const editingTask = ref<Task | null>(null)

const isAdmin = () => {
  return authStore.user?.role === 'Admin'
}

const openTaskForm = () => {
  editingTask.value = null
  showTaskForm.value = true
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

const startEditingTask = (task: Task) => {
  editingTask.value = task

  showTaskForm.value = true
}

const handleTaskCreated = async () => {
  showTaskForm.value = false

  editingTask.value = null

  await loadTasks()
}

const handleTaskUpdated = async () => {
  showTaskForm.value = false

  editingTask.value = null

  await loadTasks()
}

const cancelTaskForm = () => {
  showTaskForm.value = false

  editingTask.value = null
}

const handleStatusUpdate = async (taskId: number, status: number) => {
  try {
    await taskService.updateStatus(taskId, status)

    await loadTasks()
  } catch (err) {
    console.error(err)

    error.value = 'Unable to update task status.'
  }
}

const handleDeleteTask = async (task: Task) => {
  const confirmed = window.confirm(`Are you sure you want to delete "${task.title}"?`)

  if (!confirmed) {
    return
  }

  error.value = ''

  try {
    await taskService.deleteTask(task.id)

    await loadTasks()
  } catch (err) {
    console.error(err)

    error.value = 'Unable to delete the task.'
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
      <h1 class="text-2xl font-semibold text-gray-900">Tasks</h1>

      <button
        v-if="!isAdmin() || userSelectionStore.selectedUser"
        @click="openTaskForm"
        class="px-4 py-2 rounded bg-blue-600 text-white"
      >
        Create Task
      </button>
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

      <!-- Create form -->
      <!-- <CreateTaskForm
        v-if="showCreateForm"
        @created="handleTaskCreated"
        @cancel="showCreateForm = false"
        class="mb-6"
      /> -->

      <TaskForm
        v-if="showTaskForm"
        :task="editingTask"
        @created="handleTaskCreated"
        @updated="handleTaskUpdated"
        @cancel="cancelTaskForm"
        class="mb-6"
      />

      <!-- Task list -->
      <TaskList
        :tasks="tasks"
        @update-status="handleStatusUpdate"
        @edit="startEditingTask"
        @delete="handleDeleteTask"
      />
    </div>
  </div>
</template>
