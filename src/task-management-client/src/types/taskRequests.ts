export interface CreateTaskRequest {
  title: string

  description?: string

  status: number

  priority: number

  dueDate: string
}

export interface UpdateTaskRequest {
  title: string

  description?: string

  status: number

  priority: number

  dueDate: string
}
