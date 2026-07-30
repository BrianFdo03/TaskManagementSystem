<script setup lang="ts">
import { reactive, ref } from 'vue'

import taskService from '@/services/taskService'

import type { CreateTaskRequest } from '@/types/taskRequests'

const emit = defineEmits<{
  created: []
  cancel: []
}>()

const form = reactive<CreateTaskRequest>({
  title: '',
  description: '',
  status: 0,
  priority: 1,
  dueDate: '',
})

const submitting = ref(false)

const error = ref('')

const submit = async () => {
  error.value = ''

  if (!form.title.trim()) {
    error.value = 'Title is required.'
    return
  }

  if (!form.dueDate) {
    error.value = 'Due date is required.'
    return
  }

  submitting.value = true

  try {
    await taskService.createTask({
      title: form.title.trim(),
      description: form.description?.trim() || undefined,
      status: form.status,
      priority: form.priority,
      dueDate: form.dueDate,
    })

    emit('created')

    form.title = ''
    form.description = ''
    form.status = 0
    form.priority = 1
    form.dueDate = ''
  } catch (err) {
    console.error(err)

    error.value = 'Unable to create the task.'
  } finally {
    submitting.value = false
  }
}
</script>

<template>
  <div class="bg-white p-6 rounded shadow">
    <h2 class="text-xl font-semibold mb-4">Create Task</h2>

    <form @submit.prevent="submit" class="space-y-4">
      <!-- Title -->

      <div>
        <label for="task-title" class="block text-sm font-medium mb-1"> Title </label>

        <input
          id="task-title"
          v-model="form.title"
          type="text"
          class="w-full border rounded px-3 py-2"
          placeholder="Enter task title"
        />
      </div>

      <!-- Description -->

      <div>
        <label for="task-description" class="block text-sm font-medium mb-1"> Description </label>

        <textarea
          id="task-description"
          v-model="form.description"
          rows="3"
          class="w-full border rounded px-3 py-2"
          placeholder="Enter task description"
        ></textarea>
      </div>

      <!-- Status -->

      <div>
        <label for="task-status" class="block text-sm font-medium mb-1"> Status </label>

        <select
          id="task-status"
          v-model.number="form.status"
          class="w-full border rounded px-3 py-2"
        >
          <option :value="0">Pending</option>

          <option :value="1">In Progress</option>

          <option :value="2">Completed</option>
        </select>
      </div>

      <!-- Priority -->

      <div>
        <label for="task-priority" class="block text-sm font-medium mb-1"> Priority </label>

        <select
          id="task-priority"
          v-model.number="form.priority"
          class="w-full border rounded px-3 py-2"
        >
          <option :value="1">Low</option>

          <option :value="2">Medium</option>

          <option :value="3">High</option>
        </select>
      </div>

      <!-- Due Date -->

      <div>
        <label for="task-due-date" class="block text-sm font-medium mb-1"> Due Date </label>

        <input
          id="task-due-date"
          v-model="form.dueDate"
          type="date"
          class="w-full border rounded px-3 py-2"
        />
      </div>

      <!-- Error -->

      <div v-if="error" class="text-red-500 text-sm">
        {{ error }}
      </div>

      <!-- Buttons -->

      <div class="flex gap-3">
        <button
          type="submit"
          :disabled="submitting"
          class="px-4 py-2 rounded bg-blue-600 text-white disabled:opacity-50"
        >
          {{ submitting ? 'Creating...' : 'Create Task' }}
        </button>

        <button
          type="button"
          :disabled="submitting"
          @click="emit('cancel')"
          class="px-4 py-2 rounded border"
        >
          Cancel
        </button>
      </div>
    </form>
  </div>
</template>
