using System.Security.Claims;
using IncomeIxpenseManager.DTOs.Transactions;
using IncomeIxpenseManager.Models;
using IncomeIxpenseManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IncomeIxpenseManager.Controllers;

[Authorize]
[ApiController]
[Route("api/transactions")]
public sealed class TransactionsController(ITransactionService transactionService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<TransactionResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<TransactionResponse>>> GetAll(
        [FromQuery] TransactionFilterRequest filter,
        CancellationToken cancellationToken = default)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var transactions = await transactionService.GetAllAsync(
            userId,
            filter,
            cancellationToken);

        return Ok(transactions);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<TransactionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var transaction = await transactionService.GetByIdAsync(
            userId,
            id,
            cancellationToken);

        if (transaction is null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "İşlem bulunamadı.",
                Detail = "İşlem mevcut değil veya bu kullanıcıya ait değil."
            });
        }

        return Ok(transaction);
    }

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

    [HttpPut("{id:int}")]
    [ProducesResponseType<TransactionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TransactionResponse>> Update(
        int id,
        UpdateTransactionRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await transactionService.UpdateAsync(
            userId,
            id,
            request,
            cancellationToken);

        return result.Status switch
        {
            TransactionOperationStatus.Success => Ok(result.Transaction),
            TransactionOperationStatus.NotFound => TransactionNotFoundProblem(),
            TransactionOperationStatus.CategoryNotFound => CategoryNotFoundProblem(),
            TransactionOperationStatus.CategoryInactive => ConflictProblem(
                "İşlem güncellenemedi.",
                "Pasif bir kategoriye işlem taşınamaz."),
            _ => ConflictProblem(
                "Kategori türü uyumsuz.",
                "İşlem türü, seçilen kategorinin türüyle aynı olmalıdır.")
        };
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var status = await transactionService.DeleteAsync(userId, id, cancellationToken);

        return status == TransactionOperationStatus.NotFound
            ? TransactionNotFoundProblem()
            : NoContent();
    }

    private ObjectResult TransactionNotFoundProblem()
    {
        return NotFound(new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "İşlem bulunamadı.",
            Detail = "İşlem mevcut değil veya bu kullanıcıya ait değil."
        });
    }

    private ObjectResult CategoryNotFoundProblem()
    {
        return NotFound(new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Kategori bulunamadı.",
            Detail = "Kategori mevcut değil veya bu kullanıcıya ait değil."
        });
    }

    private ObjectResult ConflictProblem(string title, string detail)
    {
        return Conflict(new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = title,
            Detail = detail
        });
    }

    private bool TryGetUserId(out int userId)
    {
        var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(claimValue, out userId) && userId > 0;
    }
}
