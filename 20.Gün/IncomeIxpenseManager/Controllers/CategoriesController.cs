using System.Security.Claims;
using IncomeIxpenseManager.DTOs.Categories;
using IncomeIxpenseManager.Models;
using IncomeIxpenseManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IncomeIxpenseManager.Controllers;

[Authorize]
[ApiController]
[Route("api/categories")]
public sealed class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<CategoryResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<CategoryResponse>>> GetAll(
        [FromQuery] TransactionType? type,
        [FromQuery] bool? isActive,
        CancellationToken cancellationToken = default)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var categories = await categoryService.GetAllAsync(
            userId,
            type,
            isActive,
            cancellationToken);

        return Ok(categories);
    }

    [HttpPost]
    [ProducesResponseType<CategoryResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CategoryResponse>> Create(
        CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var category = await categoryService.CreateAsync(userId, request, cancellationToken);

        if (category is null)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Kategori oluşturulamadı.",
                Detail = "Aynı ad ve türde bir kategori zaten mevcut."
            });
        }

        return StatusCode(StatusCodes.Status201Created, category);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType<CategoryResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CategoryResponse>> Update(
        int id,
        UpdateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await categoryService.UpdateAsync(userId, id, request, cancellationToken);

        return result.Status switch
        {
            CategoryOperationStatus.Success => Ok(result.Category),
            CategoryOperationStatus.NotFound => NotFoundProblem(),
            CategoryOperationStatus.TypeInUse => ConflictProblem(
                "Kategori türü değiştirilemedi.",
                "İşlem kaydı bulunan bir kategorinin türü değiştirilemez."),
            _ => ConflictProblem(
                "Kategori güncellenemedi.",
                "Aynı ad ve türde bir kategori zaten mevcut.")
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

        var status = await categoryService.DeleteAsync(userId, id, cancellationToken);

        return status == CategoryOperationStatus.NotFound
            ? NotFoundProblem()
            : NoContent();
    }

    private ObjectResult NotFoundProblem()
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
