import { jwtDecode } from 'jwt-decode'

import type { JwtPayload } from '@/types/jwt'
import type { AuthUser } from '@/types/authUser'

export function decodeUser(token: string): AuthUser {
  const payload = jwtDecode<JwtPayload>(token)

  return {
    id: Number(payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']),

    email: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],

    role: payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
  }
}
