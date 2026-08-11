using IncomeIxpenseManager.Data;
using IncomeIxpenseManager.DTOs.Dashboard;
using IncomeIxpenseManager.Models;
using Microsoft.EntityFrameworkCore;

namespace IncomeIxpenseManager.Services;

public sealed class DashboardService(ApplicationDbContext dbContext) : IDashboardService
{
    public async Task<DashboardSummaryResponse> GetSummaryAsync(
        int userId,
        DashboardSummaryFilterRequest filter,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Transactions
            .AsNoTracking()
            .Where(transaction => transaction.UserId == userId);

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(transaction =>
                transaction.CategoryId == filter.CategoryId.Value);
        }

        if (filter.StartDate.HasValue)
        {
            query = query.Where(transaction =>
                transaction.TransactionDate >= filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            query = query.Where(transaction =>
                transaction.TransactionDate <= filter.EndDate.Value);
        }

        var totals = await query
            .GroupBy(_ => 1)
            .Select(group => new
            {
                TotalIncome = group.Sum(transaction =>
                    transaction.Type == TransactionType.Income ? transaction.Amount : 0m),
                TotalExpense = group.Sum(transaction =>
                    transaction.Type == TransactionType.Expense ? transaction.Amount : 0m)
            })
            .SingleOrDefaultAsync(cancellationToken);

        var totalIncome = totals?.TotalIncome ?? 0m;
        var totalExpense = totals?.TotalExpense ?? 0m;

        return new DashboardSummaryResponse(
            totalIncome,
            totalExpense,
            totalIncome - totalExpense);
    }
}
