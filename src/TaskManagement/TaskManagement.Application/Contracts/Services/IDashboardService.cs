using TaskManagement.Application.DTOs.Dashboard.Responses;

namespace TaskManagement.Application.Contracts.Services;

public interface IDashboardService
{
    Task<DashboardResponse> GetDashboardAsync(
        int userId,
        bool isAdmin);
}