import api from './api'
import type { LoginRequest, LoginResponse } from '@/types/auth'

const login = async (credentials: LoginRequest): Promise<LoginResponse> => {
  const response = await api.post<LoginResponse>('/auth/login', credentials, {
    skipAuthRedirect: true,
  })

  return response.data
}

export default {
  login,
}
