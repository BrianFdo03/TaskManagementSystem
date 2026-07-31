import api from './api'
import type { CreateUserRequest, UpdateUserRequest, User } from '@/types/user'

const getUsers = async (): Promise<User[]> => {
  const response = await api.get<User[]>('/user')

  return response.data
}
const createUser = async (request: CreateUserRequest): Promise<number> => {
  const response = await api.post<{ id: number }>('/user', request)

  return response.data.id
}

const updateUser = async (id: number, request: UpdateUserRequest): Promise<void> => {
  await api.put(`/user/${id}`, request)
}

const deleteUser = async (id: number): Promise<void> => {
  await api.delete(`/user/${id}`)
}

export default {
  getUsers,
  createUser,
  updateUser,
  deleteUser,
}
