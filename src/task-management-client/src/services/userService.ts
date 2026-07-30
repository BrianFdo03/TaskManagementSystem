import api from './api'
import type { User } from '@/types/user'

const getUsers = async (): Promise<User[]> => {
  const response = await api.get<User[]>('/user')

  return response.data
}

export default {
  getUsers,
}
