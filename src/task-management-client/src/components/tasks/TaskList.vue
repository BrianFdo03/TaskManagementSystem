<script setup lang="ts">
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
</script>

<template>
  <div>
    <div v-if="tasks.length === 0" class="bg-white p-6 rounded shadow">
      <p class="text-gray-600">No tasks found.</p>
    </div>

    <div v-else class="space-y-4">
      <div v-for="task in tasks" :key="task.id" class="bg-white p-4 rounded shadow">
        <h2 class="text-lg font-bold">
          {{ task.title }}
        </h2>

        <p v-if="task.description" class="text-gray-600 mt-1">
          {{ task.description }}
        </p>

        <div class="mt-3 space-y-1">
          <div>
            <label :for="`status-${task.id}`" class="text-sm font-medium mr-2"> Status: </label>

            <select
              :id="`status-${task.id}`"
              :value="getStatusValue(task.status)"
              @change="
                emit('updateStatus', task.id, Number(($event.target as HTMLSelectElement).value))
              "
              class="border rounded px-2 py-1"
            >
              <option :value="0">Pending</option>

              <option :value="1">In Progress</option>

              <option :value="2">Completed</option>
            </select>
          </div>

          <p>
            Priority:

            <span class="font-medium">
              {{ task.priority }}
            </span>
          </p>

          <p>
            Due Date:

            <span class="font-medium">
              {{ new Date(task.dueDate).toLocaleDateString() }}
            </span>
          </p>
        </div>
        <div class="mt-4">
          <button type="button" @click="emit('edit', task)" class="px-3 py-1 rounded border">
            Edit
          </button>
          <button type="button" @click="emit('delete', task)" class="px-3 py-1 rounded border">
            Delete
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
