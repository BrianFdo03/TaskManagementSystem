<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue'
import { CalendarDays, Filter, Plus, Search, UserRound, X } from 'lucide-vue-next'

import TaskList from '@/components/tasks/TaskList.vue'
import TaskFormModal from '@/components/tasks/TaskFormModal.vue'

import taskService from '@/services/taskService'
import userService from '@/services/userService'

import type { Task } from '@/types/task'
import type { User } from '@/types/user'

import { useUserSelectionStore } from '@/stores/userSelection'
import { useAuthStore } from '@/stores/auth'

const userSelectionStore = useUserSelectionStore()
const authStore = useAuthStore()

const tasks = ref<Task[]>([])
const users = ref<User[]>([])

const loading = ref(false)
const loadingUsers = ref(false)
const error = ref('')

const showTaskForm = ref(false)
const editingTask = ref<Task | null>(null)

const searchQuery = ref('')
const statusFilter = ref('all')
const priorityFilter = ref('all')
const dueDateFilter = ref('all')

const showFilters = ref(false)

const isAdmin = computed(() => {
  return authStore.user?.role === 'Admin'
})

/*
 * Load users for the Admin selector.
 */
const loadUsers = async () => {
  if (!isAdmin.value) {
    return
  }

  loadingUsers.value = true

  try {
    users.value = await userService.getUsers()
  } catch (err) {
    console.error(err)
    error.value = 'Unable to load users.'
  } finally {
    loadingUsers.value = false
  }
}

/*
 * Load tasks.
 *
 * Employees:
 *     GET /api/tasks
 *
 * Admins:
 *     GET /api/tasks/user/{userId}
 */
const loadTasks = async () => {
  tasks.value = []
  error.value = ''

  if (isAdmin.value && !userSelectionStore.selectedUser) {
    return
  }

  loading.value = true

  try {
    if (isAdmin.value && userSelectionStore.selectedUser) {
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

/*
 * Admin selects a user directly from the Tasks page.
 */
const selectUser = (userId: string) => {
  if (!userId) {
    userSelectionStore.clearSelection()
    return
  }

  const user = users.value.find((item) => item.id === Number(userId))

  if (user) {
    userSelectionStore.selectUser(user)
  }
}

/*
 * Client-side task filtering.
 */
const filteredTasks = computed(() => {
  let result = [...tasks.value]

  /*
   * Search
   */
  const search = searchQuery.value.trim().toLowerCase()

  if (search) {
    result = result.filter((task) => {
      return (
        task.title.toLowerCase().includes(search) ||
        (task.description ?? '').toLowerCase().includes(search)
      )
    })
  }

  /*
   * Status
   */
  if (statusFilter.value !== 'all') {
    result = result.filter(
      (task) => task.status.toLowerCase().replace(' ', '') === statusFilter.value,
    )
  }

  /*
   * Priority
   */
  if (priorityFilter.value !== 'all') {
    result = result.filter((task) => task.priority.toLowerCase() === priorityFilter.value)
  }

  /*
   * Due date
   */
  if (dueDateFilter.value !== 'all') {
    const today = new Date()
    today.setHours(0, 0, 0, 0)

    result = result.filter((task) => {
      const dueDate = new Date(task.dueDate)
      dueDate.setHours(0, 0, 0, 0)

      switch (dueDateFilter.value) {
        case 'overdue':
          return dueDate < today

        case 'today':
          return dueDate.getTime() === today.getTime()

        case 'upcoming':
          return dueDate > today

        default:
          return true
      }
    })
  }

  return result
})

const hasActiveFilters = computed(() => {
  return (
    searchQuery.value.trim() !== '' ||
    statusFilter.value !== 'all' ||
    priorityFilter.value !== 'all' ||
    dueDateFilter.value !== 'all'
  )
})

const clearFilters = () => {
  searchQuery.value = ''
  statusFilter.value = 'all'
  priorityFilter.value = 'all'
  dueDateFilter.value = 'all'
}

const openTaskForm = () => {
  editingTask.value = null
  showTaskForm.value = true
}

const startEditingTask = (task: Task) => {
  editingTask.value = task
  showTaskForm.value = true
}

const cancelTaskForm = () => {
  showTaskForm.value = false
  editingTask.value = null
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

const handleStatusUpdate = async (taskId: number, status: number) => {
  error.value = ''

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

/*
 * Initial loading.
 */
onMounted(async () => {
  await loadUsers()
  await loadTasks()
})

/*
 * Reload tasks whenever the Admin changes users.
 */
watch(
  () => userSelectionStore.selectedUser?.id,
  () => {
    if (isAdmin.value) {
      loadTasks()
    }
  },
)
</script>

<template>
  <div class="space-y-5">
    <!-- Header -->
    <div class="flex items-center justify-between gap-3">
      <div>
        <h1 class="text-2xl font-semibold text-gray-900">Tasks</h1>

        <p class="mt-1 text-sm text-gray-500">Manage and track your tasks.</p>
      </div>

      <button
        v-if="!isAdmin || userSelectionStore.selectedUser"
        type="button"
        @click="openTaskForm"
        class="flex shrink-0 items-center gap-2 rounded-xl bg-blue-600 px-4 py-2.5 text-sm font-medium text-white shadow-sm transition hover:bg-blue-700"
      >
        <Plus :size="18" />

        <span class="hidden sm:inline"> Create Task </span>
      </button>
    </div>

    <!-- Admin User Selector -->
    <div v-if="isAdmin" class="rounded-2xl border border-gray-100 bg-white p-4 shadow-sm">
      <label for="task-user" class="mb-2 flex items-center gap-2 text-sm font-medium text-gray-700">
        <UserRound :size="17" class="text-blue-600" />

        Select User
      </label>

      <select
        id="task-user"
        :value="userSelectionStore.selectedUser?.id ?? ''"
        :disabled="loadingUsers"
        @change="selectUser(($event.target as HTMLSelectElement).value)"
        class="w-full rounded-xl border border-gray-200 bg-gray-50 px-4 py-3 text-sm text-gray-900 outline-none transition focus:border-blue-500 focus:bg-white focus:ring-2 focus:ring-blue-100"
      >
        <option value="">Select a user to view tasks</option>

        <option v-for="user in users" :key="user.id" :value="user.id">
          {{ user.name }} — {{ user.email }}
        </option>
      </select>

      <p v-if="userSelectionStore.selectedUser" class="mt-2 text-xs text-gray-500">
        Showing tasks for
        <span class="font-medium text-gray-700">
          {{ userSelectionStore.selectedUser.name }}
        </span>
      </p>
    </div>

    <!-- Employee / Admin task area -->
    <template v-if="!isAdmin || userSelectionStore.selectedUser">
      <!-- Search -->
      <div class="relative">
        <Search :size="19" class="absolute left-4 top-1/2 -translate-y-1/2 text-gray-400" />

        <input
          v-model="searchQuery"
          type="search"
          placeholder="Search tasks..."
          class="w-full rounded-2xl border border-gray-100 bg-white py-3.5 pl-11 pr-4 text-sm text-gray-900 shadow-sm outline-none transition placeholder:text-gray-400 focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
        />
      </div>

      <!-- Filter Controls -->
      <div class="flex items-center justify-between gap-3">
        <button
          type="button"
          @click="showFilters = !showFilters"
          class="flex items-center gap-2 rounded-xl border border-gray-200 bg-white px-4 py-2.5 text-sm font-medium text-gray-700 shadow-sm transition hover:bg-gray-50"
        >
          <Filter :size="17" />

          Filters

          <span
            v-if="hasActiveFilters"
            class="flex h-5 min-w-5 items-center justify-center rounded-full bg-blue-600 px-1.5 text-xs font-semibold text-white"
          >
            !
          </span>
        </button>

        <button
          v-if="hasActiveFilters"
          type="button"
          @click="clearFilters"
          class="flex items-center gap-1 text-sm font-medium text-blue-600 hover:text-blue-700"
        >
          <X :size="15" />

          Clear
        </button>
      </div>

      <!-- Filters -->
      <div
        v-if="showFilters"
        class="grid grid-cols-1 gap-3 rounded-2xl border border-gray-100 bg-white p-4 shadow-sm sm:grid-cols-3"
      >
        <!-- Status -->
        <div>
          <label for="status-filter" class="mb-1.5 block text-xs font-medium text-gray-500">
            Status
          </label>

          <select
            id="status-filter"
            v-model="statusFilter"
            class="w-full rounded-xl border border-gray-200 bg-gray-50 px-3 py-2.5 text-sm outline-none focus:border-blue-500"
          >
            <option value="all">All statuses</option>
            <option value="pending">Pending</option>
            <option value="inprogress">In Progress</option>
            <option value="completed">Completed</option>
          </select>
        </div>

        <!-- Priority -->
        <div>
          <label for="priority-filter" class="mb-1.5 block text-xs font-medium text-gray-500">
            Priority
          </label>

          <select
            id="priority-filter"
            v-model="priorityFilter"
            class="w-full rounded-xl border border-gray-200 bg-gray-50 px-3 py-2.5 text-sm outline-none focus:border-blue-500"
          >
            <option value="all">All priorities</option>
            <option value="low">Low</option>
            <option value="medium">Medium</option>
            <option value="high">High</option>
          </select>
        </div>

        <!-- Due Date -->
        <div>
          <label
            for="due-date-filter"
            class="mb-1.5 flex items-center gap-1 text-xs font-medium text-gray-500"
          >
            <CalendarDays :size="14" />

            Due date
          </label>

          <select
            id="due-date-filter"
            v-model="dueDateFilter"
            class="w-full rounded-xl border border-gray-200 bg-gray-50 px-3 py-2.5 text-sm outline-none focus:border-blue-500"
          >
            <option value="all">All dates</option>
            <option value="overdue">Overdue</option>
            <option value="today">Due today</option>
            <option value="upcoming">Upcoming</option>
          </select>
        </div>
      </div>

      <!-- Loading -->
      <div
        v-if="loading"
        class="rounded-2xl bg-white p-8 text-center text-sm text-gray-500 shadow-sm"
      >
        Loading tasks...
      </div>

      <!-- Error -->
      <div
        v-else-if="error"
        class="rounded-2xl border border-red-100 bg-red-50 p-4 text-sm text-red-600"
      >
        {{ error }}
      </div>

      <!-- Task List -->
      <TaskList
        v-else
        :tasks="filteredTasks"
        @update-status="handleStatusUpdate"
        @edit="startEditingTask"
        @delete="handleDeleteTask"
      />
    </template>

    <!-- Admin has not selected user -->
    <div
      v-else
      class="rounded-2xl border border-gray-100 bg-white px-6 py-12 text-center shadow-sm"
    >
      <div
        class="mx-auto mb-4 flex h-12 w-12 items-center justify-center rounded-full bg-blue-50 text-blue-600"
      >
        <UserRound :size="22" />
      </div>

      <h2 class="font-semibold text-gray-900">Select a user</h2>

      <p class="mt-1 text-sm text-gray-500">Select a user above to view and manage their tasks.</p>
    </div>

    <!-- Task Modal -->
    <TaskFormModal
      v-if="showTaskForm"
      :task="editingTask"
      @created="handleTaskCreated"
      @updated="handleTaskUpdated"
      @cancel="cancelTaskForm"
    />
  </div>
</template>
