export interface User {
  id: number
  name: string
  email: string
  role: string
  createdDate: string
}

export interface CreateUserRequest {
  name: string
  email: string
  password: string
  role: number
}

export interface UpdateUserRequest {
  name: string
  email: string
  role: number
}
