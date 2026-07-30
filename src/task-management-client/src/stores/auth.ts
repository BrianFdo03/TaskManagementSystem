import { computed, ref } from 'vue'
import { defineStore } from 'pinia'
import authService from '@/services/authService'
import type { LoginRequest } from '@/types/auth'
import type { AuthUser } from '@/types/authUser'
import { decodeUser } from '@/utils/jwt'

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('auth_token'))

  const expiresAt = ref<string | null>(localStorage.getItem('auth_expires_at'))

  const user = ref<AuthUser | null>(token.value ? decodeUser(token.value) : null)

  const isAuthenticated = computed(() => {
    return token.value !== null
  })

  const login = async (credentials: LoginRequest) => {
    const response = await authService.login(credentials)

    token.value = response.token
    expiresAt.value = response.expiresAt
    user.value = decodeUser(response.token)

    localStorage.setItem('auth_token', response.token)
    localStorage.setItem('auth_expires_at', response.expiresAt)
  }

  const logout = () => {
    token.value = null
    expiresAt.value = null
    user.value = null

    localStorage.removeItem('auth_token')
    localStorage.removeItem('auth_expires_at')
  }

  return {
    token,
    expiresAt,
    user,
    isAuthenticated,
    login,
    logout,
  }
})
