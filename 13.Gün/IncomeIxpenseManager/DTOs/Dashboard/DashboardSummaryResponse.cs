namespace IncomeIxpenseManager.DTOs.Dashboard;

public sealed record DashboardSummaryResponse(
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance);
