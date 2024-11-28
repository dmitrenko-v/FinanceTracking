using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.ExceptionHandlers;

public class ForbiddenExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ForbiddenExceptionHandler> _logger;

    public ForbiddenExceptionHandler(ILogger<ForbiddenExceptionHandler> logger)
    {
        this._logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (httpContext is null)
        {
            throw new InvalidOperationException();
        }

        if (exception is not ForbiddenException forbidException)
        {
            return false;
        }

        this._logger.LogTrace(forbidException, "You have no permission");

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden, Title = "Forbidden", Detail = "You have no permission",
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
