import api from './api'

import type { Task } from '@/types/task'
import type { CreateTaskRequest, UpdateTaskRequest } from '@/types/taskRequests'

const getMyTasks = async (): Promise<Task[]> => {
  const response = await api.get<Task[]>('/tasks')

  return response.data
}

const getTasksByUser = async (userId: number): Promise<Task[]> => {
  const response = await api.get<Task[]>(`/tasks/user/${userId}`)

  return response.data
}

const createTask = async (request: CreateTaskRequest): Promise<Task> => {
  const response = await api.post<Task>('/tasks', request)

  return response.data
}

const updateTask = async (id: number, request: UpdateTaskRequest) => {
  await api.put(`/tasks/${id}`, request)
}

const updateStatus = async (id: number, status: number) => {
  await api.patch(`/tasks/${id}/status`, status)
}

const deleteTask = async (id: number) => {
  await api.delete(`/tasks/${id}`)
}

export default {
  getMyTasks,

  getTasksByUser,

  createTask,

  updateTask,

  updateStatus,

  deleteTask,
}
