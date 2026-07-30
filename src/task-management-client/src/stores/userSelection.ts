import { defineStore } from 'pinia'
import { ref } from 'vue'

import type { User } from '@/types/user'

export const useUserSelectionStore = defineStore('userSelection', () => {
  const selectedUser = ref<User | null>(null)

  const selectUser = (user: User) => {
    selectedUser.value = user
  }

  const clearSelection = () => {
    selectedUser.value = null
  }

  return {
    selectedUser,
    selectUser,
    clearSelection,
  }
})
