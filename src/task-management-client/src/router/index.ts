import AppLayout from '@/layouts/AppLayout.vue'
import { useAuthStore } from '@/stores/auth'
import DashboardView from '@/views/DashboardView.vue'
import LoginView from '@/views/LoginView.vue'
import TasksView from '@/views/TasksView.vue'
import UsersView from '@/views/UsersView.vue'
import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),

  routes: [
    {
      path: '/login',
      name: 'Login',
      component: LoginView,
      meta: {
        requiresAuth: false,
      },
    },
    {
      path: '/',
      component: AppLayout,
      meta: {
        requiresAuth: true,
      },

      children: [
        {
          path: '',
          redirect: '/dashboard',
        },

        {
          path: 'dashboard',
          name: 'Dashboard',
          component: DashboardView,
        },

        {
          path: 'tasks',
          name: 'Tasks',
          component: TasksView,
        },

        {
          path: 'users',
          name: 'Users',
          component: UsersView,

          meta: {
            roles: ['Admin'],
          },
        },
      ],
    },
  ],
})

router.beforeEach((to) => {
  const authStore = useAuthStore()

  /*
   * Authentication check
   */
  const requiresAuth = to.matched.some((record) => record.meta.requiresAuth)

  if (requiresAuth && !authStore.isAuthenticated) {
    return {
      name: 'Login',
    }
  }

  /*
   * Authorization check
   */
  const requiredRoles = to.matched.flatMap((record) => record.meta.roles ?? [])

  if (requiredRoles.length > 0) {
    const userRole = authStore.user?.role

    const hasRole = userRole && requiredRoles.includes(userRole)

    if (!hasRole) {
      return {
        name: 'Dashboard',
      }
    }
  }

  return true
})

export default router
