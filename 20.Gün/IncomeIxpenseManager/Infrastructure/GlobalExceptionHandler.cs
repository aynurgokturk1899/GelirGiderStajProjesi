using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace IncomeIxpenseManager.Infrastructure;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService,
    IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            exception,
            "İstek işlenirken beklenmeyen bir hata oluştu. TraceId: {TraceId}",
            httpContext.TraceIdentifier);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var detail = environment.IsDevelopment()
            ? $"{exception.GetBaseException().Message} (TraceId: {httpContext.TraceIdentifier})"
            : "İstek işlenirken beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Sunucu hatası.",
                Detail = detail
            },
            Exception = exception
        });
    }
}
