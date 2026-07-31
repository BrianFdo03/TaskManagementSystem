import api from './api'
import type { Dashboard } from '@/types/dashboard'

const getDashboard = async (): Promise<Dashboard> => {
  const response = await api.get<Dashboard>('/dashboard')

  return response.data
}

export default {
  getDashboard,
}
