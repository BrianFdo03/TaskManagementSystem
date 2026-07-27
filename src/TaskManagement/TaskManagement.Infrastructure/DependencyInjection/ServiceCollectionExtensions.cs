using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Infrastructure.Database;
using TaskManagement.Infrastructure.Execution;
using TaskManagement.Infrastructure.QueryManagement;
using TaskManagement.Infrastructure.Repositories;
using TaskManagement.Application.Contracts.Services;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Infrastructure.Security;

namespace TaskManagement.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Database
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        // Database Executor
        services.AddScoped<IDatabaseExecutor, DatabaseExecutor>();

        // Query system
        services.AddSingleton<ISqlQueryService, SqlQueryService>();

        // Security
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthRepository, AuthRepository>();

        return services;
    }
}