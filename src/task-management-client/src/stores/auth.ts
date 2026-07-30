import { computed, ref } from "vue";
import { defineStore } from "pinia";
import authService from "@/services/authService";
import type { LoginRequest } from "@/types/auth";

export const useAuthStore = defineStore("auth", () => {
    const token = ref<string | null>(
        localStorage.getItem("auth_token")
    );

    const expiresAt = ref<string | null>(
        localStorage.getItem("auth_expires_at")
    );

    const isAuthenticated = computed(() => {
        return token.value !== null;
    });

    const login = async (credentials: LoginRequest) => {
        const response = await authService.login(credentials);

        token.value = response.token;
        expiresAt.value = response.expiresAt;

        localStorage.setItem("auth_token", response.token);
        localStorage.setItem(
            "auth_expires_at",
            response.expiresAt
        );
    };

    const logout = () => {
        token.value = null;
        expiresAt.value = null;

        localStorage.removeItem("auth_token");
        localStorage.removeItem("auth_expires_at");
    };

    return {
        token,
        expiresAt,
        isAuthenticated,
        login,
        logout,
    };
});