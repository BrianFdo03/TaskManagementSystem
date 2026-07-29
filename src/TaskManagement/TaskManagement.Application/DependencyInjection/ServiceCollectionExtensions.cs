using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Contracts.Services;
using TaskManagement.Application.Services;

namespace TaskManagement.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITaskService, TaskService>();

        return services;
    }
}