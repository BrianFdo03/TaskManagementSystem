<script setup lang="ts">
import type { Task } from '@/types/task'

defineProps<{
  tasks: Task[]
}>()
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
          <p>
            Status:

            <span class="font-medium">
              {{ task.status }}
            </span>
          </p>

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
      </div>
    </div>
  </div>
</template>
