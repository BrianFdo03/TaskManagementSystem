```vue
<script setup lang="ts">
import { reactive, ref, watch } from 'vue'
import { X } from 'lucide-vue-next'

import taskService from '@/services/taskService'
import { useUserSelectionStore } from '@/stores/userSelection'
import { useAuthStore } from '@/stores/auth'

import type { Task } from '@/types/task'
import type { CreateTaskRequest } from '@/types/taskRequests'

const userSelectionStore = useUserSelectionStore()
const authStore = useAuthStore()

const props = defineProps<{
  task?: Task | null
}>()

const emit = defineEmits<{
  created: []
  updated: []
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

const isEditMode = () => {
  return props.task !== null && props.task !== undefined
}

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

const getPriorityValue = (priority: string): number => {
  switch (priority.toLowerCase()) {
    case 'low':
      return 1

    case 'medium':
      return 2

    case 'high':
      return 3

    default:
      return 1
  }
}

const populateForm = () => {
  error.value = ''

  if (!props.task) {
    form.title = ''
    form.description = ''
    form.status = 0
    form.priority = 1
    form.dueDate = ''

    return
  }

  form.title = props.task.title
  form.description = props.task.description ?? ''
  form.status = getStatusValue(props.task.status)
  form.priority = getPriorityValue(props.task.priority)
  form.dueDate = props.task.dueDate.substring(0, 10)
}

watch(
  () => props.task,
  () => {
    populateForm()
  },
  {
    immediate: true,
  },
)

const closeModal = () => {
  if (submitting.value) {
    return
  }

  emit('cancel')
}

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
    const request: CreateTaskRequest = {
      title: form.title.trim(),
      description: form.description?.trim() || undefined,
      status: form.status,
      priority: form.priority,
      dueDate: form.dueDate,
    }

    /*
     * When an Admin creates a task, assign it to
     * the currently selected user.
     */
    if (authStore.user?.role === 'Admin' && userSelectionStore.selectedUser) {
      request.userId = userSelectionStore.selectedUser.id
    }

    if (isEditMode()) {
      await taskService.updateTask(props.task!.id, request)

      emit('updated')
    } else {
      await taskService.createTask(request)

      emit('created')
    }
  } catch (err) {
    console.error(err)

    error.value = isEditMode() ? 'Unable to update the task.' : 'Unable to create the task.'
  } finally {
    submitting.value = false
  }
}
</script>

<template>
  <!-- Modal backdrop -->
  <div
    class="fixed inset-0 z-50 flex items-end sm:items-center justify-center bg-black/40 px-0 sm:px-4"
    @click.self="closeModal"
  >
    <!-- Modal -->
    <div
      class="w-full sm:max-w-lg max-h-[92vh] overflow-y-auto rounded-t-3xl sm:rounded-2xl bg-white shadow-xl"
    >
      <!-- Header -->
      <div
        class="sticky top-0 z-10 flex items-center justify-between border-b border-gray-100 bg-white px-5 py-4"
      >
        <div>
          <h2 class="text-lg font-semibold text-gray-900">
            {{ isEditMode() ? 'Edit Task' : 'Create Task' }}
          </h2>

          <p class="mt-0.5 text-sm text-gray-500">
            {{
              isEditMode() ? 'Update the task details below.' : 'Add a new task to the task list.'
            }}
          </p>
        </div>

        <button
          type="button"
          :disabled="submitting"
          @click="closeModal"
          class="flex h-9 w-9 items-center justify-center rounded-full text-gray-500 transition hover:bg-gray-100 hover:text-gray-700 disabled:opacity-50"
          aria-label="Close"
        >
          <X :size="20" />
        </button>
      </div>

      <!-- Form -->
      <form @submit.prevent="submit" class="space-y-5 px-5 py-5">
        <!-- Title -->
        <div>
          <label for="task-title" class="mb-1.5 block text-sm font-medium text-gray-700">
            Title
          </label>

          <input
            id="task-title"
            v-model="form.title"
            type="text"
            placeholder="Enter task title"
            class="w-full rounded-xl border border-gray-200 bg-gray-50 px-4 py-3 text-sm text-gray-900 outline-none transition focus:border-blue-500 focus:bg-white focus:ring-2 focus:ring-blue-100"
          />
        </div>

        <!-- Description -->
        <div>
          <label for="task-description" class="mb-1.5 block text-sm font-medium text-gray-700">
            Description
          </label>

          <textarea
            id="task-description"
            v-model="form.description"
            rows="4"
            placeholder="Enter task description"
            class="w-full resize-none rounded-xl border border-gray-200 bg-gray-50 px-4 py-3 text-sm text-gray-900 outline-none transition focus:border-blue-500 focus:bg-white focus:ring-2 focus:ring-blue-100"
          ></textarea>
        </div>

        <!-- Status and Priority -->
        <div class="grid grid-cols-1 gap-4 sm:grid-cols-2">
          <!-- Status -->
          <div>
            <label for="task-status" class="mb-1.5 block text-sm font-medium text-gray-700">
              Status
            </label>

            <select
              id="task-status"
              v-model.number="form.status"
              class="w-full rounded-xl border border-gray-200 bg-gray-50 px-4 py-3 text-sm text-gray-900 outline-none transition focus:border-blue-500 focus:bg-white focus:ring-2 focus:ring-blue-100"
            >
              <option :value="0">Pending</option>
              <option :value="1">In Progress</option>
              <option :value="2">Completed</option>
            </select>
          </div>

          <!-- Priority -->
          <div>
            <label for="task-priority" class="mb-1.5 block text-sm font-medium text-gray-700">
              Priority
            </label>

            <select
              id="task-priority"
              v-model.number="form.priority"
              class="w-full rounded-xl border border-gray-200 bg-gray-50 px-4 py-3 text-sm text-gray-900 outline-none transition focus:border-blue-500 focus:bg-white focus:ring-2 focus:ring-blue-100"
            >
              <option :value="1">Low</option>
              <option :value="2">Medium</option>
              <option :value="3">High</option>
            </select>
          </div>
        </div>

        <!-- Due Date -->
        <div>
          <label for="task-due-date" class="mb-1.5 block text-sm font-medium text-gray-700">
            Due Date
          </label>

          <input
            id="task-due-date"
            v-model="form.dueDate"
            type="date"
            class="w-full rounded-xl border border-gray-200 bg-gray-50 px-4 py-3 text-sm text-gray-900 outline-none transition focus:border-blue-500 focus:bg-white focus:ring-2 focus:ring-blue-100"
          />
        </div>

        <!-- Error -->
        <div
          v-if="error"
          class="rounded-xl border border-red-100 bg-red-50 px-4 py-3 text-sm text-red-600"
        >
          {{ error }}
        </div>

        <!-- Actions -->
        <div class="flex gap-3 border-t border-gray-100 pt-5">
          <button
            type="button"
            :disabled="submitting"
            @click="closeModal"
            class="flex-1 rounded-xl border border-gray-200 px-4 py-3 text-sm font-medium text-gray-700 transition hover:bg-gray-50 disabled:opacity-50"
          >
            Cancel
          </button>

          <button
            type="submit"
            :disabled="submitting"
            class="flex-1 rounded-xl bg-blue-600 px-4 py-3 text-sm font-medium text-white transition hover:bg-blue-700 disabled:cursor-not-allowed disabled:opacity-50"
          >
            {{ submitting ? 'Saving...' : isEditMode() ? 'Update Task' : 'Create Task' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>
```
