import AppLayout from '@/layouts/AppLayout.vue';
import { useAuthStore } from '@/stores/auth';
import DashboardView from '@/views/DashboardView.vue';
import LoginView from '@/views/LoginView.vue'
import TasksView from '@/views/TasksView.vue';
import UsersView from '@/views/UsersView.vue';
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
      path: "/app",
      component: AppLayout,
      meta: {
        requiresAuth: true,
      },

      children: [
        {
          path: "",
          redirect: "/app/dashboard",
        },

        {
          path: "dashboard",
          name: "Dashboard",
          component: DashboardView,
        },

        {
          path: "tasks",
          name: "Tasks",
          component: TasksView,
        },

        {
          path: "users",
          name: "Users",
          component: UsersView,
        },
      ],
    },
  ],
})

router.beforeEach((to) => {
    const authStore = useAuthStore();

    const requiresAuth = to.matched.some(
        (record) => record.meta.requiresAuth
    );


    if (
        requiresAuth  && !authStore.isAuthenticated
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