import { useAuthStore } from '@/stores/auth';
import DashboardView from '@/views/DashboardView.vue'
import LoginView from '@/views/LoginView.vue'
import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),

  routes: [
    {
      path: '/',
      redirect: '/login',
    },
    {
      path: "/login",
      name: "Login",
      component: LoginView,
      meta: {
          requiresAuth: false,
      },
    
    },
    {
      path: "/dashboard",
      name: "Dashboard",
      component: DashboardView,
      meta: {
          requiresAuth: true,
      },
  },
  ],
})

router.beforeEach((to) => {
    const authStore = useAuthStore();

    if (
        to.meta.requiresAuth &&
        !authStore.isAuthenticated
    ) {
        return {
            name: "Login",
        };
    }

    if (
        to.name === "Login" &&
        authStore.isAuthenticated
    ) {
        return {
            name: "Dashboard",
        };
    }

    return true;
});

export default router