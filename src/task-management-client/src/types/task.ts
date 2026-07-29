export interface Task {
  id: number
  title: string
  description: string | null
  status: string
  priority: string
  dueDate: string
  createdDate: string
  updatedDate: string | null
  userId: number
}