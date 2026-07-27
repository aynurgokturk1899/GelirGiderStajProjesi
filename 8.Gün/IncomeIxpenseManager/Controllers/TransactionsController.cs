using System.Security.Claims;
using IncomeIxpenseManager.DTOs.Transactions;
using IncomeIxpenseManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IncomeIxpenseManager.Controllers;

[Authorize]
[ApiController]
[Route("api/transactions")]
public sealed class TransactionsController(ITransactionService transactionService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<TransactionResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TransactionResponse>> Create(
        CreateTransactionRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        if (request.TransactionDate == default)
        {
            ModelState.AddModelError(
                nameof(request.TransactionDate),
                "İşlem tarihi zorunludur.");
        }

        if (request.Amount <= 0)
        {
            ModelState.AddModelError(
                nameof(request.Amount),
                "Tutar sıfırdan büyük olmalıdır.");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var result = await transactionService.CreateAsync(
            userId,
            request,
            cancellationToken);

        return result.Status switch
        {
            TransactionOperationStatus.Success => StatusCode(
                StatusCodes.Status201Created,
                result.Transaction),
            TransactionOperationStatus.CategoryNotFound => NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Kategori bulunamadı.",
                Detail = "Kategori mevcut değil veya bu kullanıcıya ait değil."
            }),
            TransactionOperationStatus.CategoryInactive => Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "İşlem eklenemedi.",
                Detail = "Pasif bir kategoriye işlem eklenemez."
            }),
            _ => Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Kategori türü uyumsuz.",
                Detail = "İşlem türü, seçilen kategorinin türüyle aynı olmalıdır."
            })
        };
    }

    private bool TryGetUserId(out int userId)
    {
        var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(claimValue, out userId) && userId > 0;
    }
}
