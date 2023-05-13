using System.Net;
using System.Text.Json;
using ResourceManager.Application.Common;
using ResourceManager.Domain.Common;

namespace ResourceManager.Api.Middlewares;

internal class ApplicationAndDomainExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ApplicationAndDomainExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex) when (ex is ApplicationLogicException or DomainException)
        {
            string errorResponse = JsonSerializer.Serialize(new
            {
                type = ex is ApplicationLogicException
                    ? nameof(ApplicationLogicException)
                    : nameof(DomainException),
                code = ex.GetType().Name,
                description = ex.Message,
            });

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(errorResponse);
        }
    }
}
