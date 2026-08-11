using IncomeIxpenseManager.DTOs.Dashboard;

namespace IncomeIxpenseManager.Services;

public interface IDashboardService
{
    Task<DashboardSummaryResponse> GetSummaryAsync(
        int userId,
        DashboardSummaryFilterRequest filter,
        CancellationToken cancellationToken);
}
