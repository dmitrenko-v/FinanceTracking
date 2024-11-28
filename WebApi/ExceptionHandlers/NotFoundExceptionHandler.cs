using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.ExceptionHandlers;

public class NotFoundExceptionHandler : IExceptionHandler
{
    private readonly ILogger<BadRequestExceptionHandler> logger;

    public NotFoundExceptionHandler(ILogger<BadRequestExceptionHandler> logger)
    {
        this.logger = logger;
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

        if (exception is not NotFoundException notFoundException)
        {
            return false;
        }

        this.logger.LogError(notFoundException, "{Message}", notFoundException.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound, Title = "Not found.", Detail = notFoundException.Message,
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
