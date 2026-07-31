<script setup lang="ts">
import { CalendarDays, CheckCircle2, Circle, Clock3, Edit3, Trash2 } from 'lucide-vue-next'

import type { Task } from '@/types/task'

defineProps<{
  tasks: Task[]
}>()

const emit = defineEmits<{
  updateStatus: [taskId: number, status: number]
  edit: [task: Task]
  delete: [task: Task]
}>()

const getStatusValue = (status: string): number => {
  switch (status.toLowerCase()) {
    case 'pending':
      return 0

    case 'inprogress':
    case 'in progress':
      return 1

    case 'completed':
      return 2

    default:
      return 0
  }
}

const getStatusLabel = (status: string): string => {
  switch (status.toLowerCase()) {
    case 'pending':
      return 'Pending'

    case 'inprogress':
    case 'in progress':
      return 'In Progress'

    case 'completed':
      return 'Completed'

    default:
      return status
  }
}

const getPriorityLabel = (priority: string): string => {
  return priority.charAt(0).toUpperCase() + priority.slice(1).toLowerCase()
}

const getPriorityClass = (priority: string): string => {
  switch (priority.toLowerCase()) {
    case 'high':
      return 'bg-red-50 text-red-600'

    case 'medium':
      return 'bg-amber-50 text-amber-600'

    case 'low':
      return 'bg-blue-50 text-blue-600'

    default:
      return 'bg-gray-50 text-gray-600'
  }
}

const getStatusClass = (status: string): string => {
  switch (status.toLowerCase()) {
    case 'pending':
      return 'bg-gray-100 text-gray-600'

    case 'inprogress':
    case 'in progress':
      return 'bg-blue-50 text-blue-600'

    case 'completed':
      return 'bg-green-50 text-green-600'

    default:
      return 'bg-gray-100 text-gray-600'
  }
}

const getStatusIcon = (status: string) => {
  switch (status.toLowerCase()) {
    case 'pending':
      return Circle

    case 'inprogress':
    case 'in progress':
      return Clock3

    case 'completed':
      return CheckCircle2

    default:
      return Circle
  }
}

const formatDate = (date: string): string => {
  return new Date(date).toLocaleDateString(undefined, {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  })
}

const isOverdue = (task: Task): boolean => {
  if (task.status.toLowerCase() === 'completed') {
    return false
  }

  const dueDate = new Date(task.dueDate)
  const today = new Date()

  today.setHours(0, 0, 0, 0)
  dueDate.setHours(0, 0, 0, 0)

  return dueDate < today
}
</script>

<template>
  <!-- Empty state -->
  <div
    v-if="tasks.length === 0"
    class="flex flex-col items-center justify-center rounded-2xl border border-gray-100 bg-white px-6 py-12 text-center shadow-sm"
  >
    <div class="mb-4 flex h-12 w-12 items-center justify-center rounded-full bg-blue-50">
      <CheckCircle2 class="h-6 w-6 text-blue-500" />
    </div>

    <h3 class="text-base font-semibold text-gray-900">No tasks found</h3>

    <p class="mt-1 max-w-sm text-sm text-gray-500">
      There are no tasks matching the current filters.
    </p>
  </div>

  <!-- Task cards -->
  <div v-else class="grid grid-cols-1 gap-4 xl:grid-cols-2">
    <article
      v-for="task in tasks"
      :key="task.id"
      class="rounded-2xl border border-gray-100 bg-white p-4 shadow-sm transition hover:shadow-md sm:p-5"
    >
      <!-- Header -->
      <div class="flex items-start justify-between gap-3">
        <div class="min-w-0 flex-1">
          <h2 class="truncate text-base font-semibold text-gray-900 sm:text-lg">
            {{ task.title }}
          </h2>

          <p v-if="task.description" class="mt-1 line-clamp-2 text-sm leading-5 text-gray-500">
            {{ task.description }}
          </p>
        </div>

        <!-- Priority -->
        <span
          class="shrink-0 rounded-full px-2.5 py-1 text-xs font-medium"
          :class="getPriorityClass(task.priority)"
        >
          {{ getPriorityLabel(task.priority) }}
        </span>
      </div>

      <!-- Status + due date -->
      <div class="mt-4 flex flex-wrap items-center gap-2">
        <!-- Status -->
        <div
          class="flex items-center gap-1.5 rounded-full px-2.5 py-1 text-xs font-medium"
          :class="getStatusClass(task.status)"
        >
          <component :is="getStatusIcon(task.status)" class="h-3.5 w-3.5" />

          {{ getStatusLabel(task.status) }}
        </div>

        <!-- Due date -->
        <div
          class="flex items-center gap-1.5 text-xs"
          :class="isOverdue(task) ? 'font-medium text-red-600' : 'text-gray-500'"
        >
          <CalendarDays class="h-3.5 w-3.5" />

          {{ isOverdue(task) ? 'Overdue · ' : 'Due · ' }}
          {{ formatDate(task.dueDate) }}
        </div>
      </div>

      <!-- Status selector -->
      <div class="mt-4">
        <label :for="`status-${task.id}`" class="mb-1.5 block text-xs font-medium text-gray-500">
          Update status
        </label>

        <select
          :id="`status-${task.id}`"
          :value="getStatusValue(task.status)"
          @change="
            emit('updateStatus', task.id, Number(($event.target as HTMLSelectElement).value))
          "
          class="w-full rounded-xl border border-gray-200 bg-white px-3 py-2.5 text-sm text-gray-700 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
        >
          <option :value="0">Pending</option>

          <option :value="1">In Progress</option>

          <option :value="2">Completed</option>
        </select>
      </div>

      <!-- Actions -->
      <div class="mt-4 flex gap-2 border-t border-gray-100 pt-4">
        <button
          type="button"
          @click="emit('edit', task)"
          class="flex flex-1 items-center justify-center gap-2 rounded-xl border border-gray-200 px-3 py-2.5 text-sm font-medium text-gray-700 transition hover:border-blue-200 hover:bg-blue-50 hover:text-blue-600"
        >
          <Edit3 class="h-4 w-4" />
          Edit
        </button>

        <button
          type="button"
          @click="emit('delete', task)"
          class="flex flex-1 items-center justify-center gap-2 rounded-xl border border-red-100 px-3 py-2.5 text-sm font-medium text-red-600 transition hover:bg-red-50"
        >
          <Trash2 class="h-4 w-4" />
          Delete
        </button>
      </div>
    </article>
  </div>
</template>
