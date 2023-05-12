using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResourceManager.Application.DTOs;
using ResourceManager.Application.Resources.Commands;
using ResourceManager.Application.Resources.Queries;

namespace ResourceManager.Api;

public static class Endpoints
{
    public static WebApplication MapResourcesEndpoints(this WebApplication app)
    {
        // Create resource
        app.MapPost(
            "resource",
            async (CreateResourceCommand command, IMediator mediator, CancellationToken ct) =>
            {
                ResourceDto resourceDto = await mediator.Send(command, ct);
                return Results.CreatedAtRoute("GetResourceById", new { id = resourceDto.Id }, resourceDto);
            }).RequireAuthorization(policy => policy.RequireRole("admin"));

        // -------------------------------------------------------------------------------------------------------------
        // Locking resource
        app.MapPut(
            "resource/{id}/lock",
            async (Guid id, [FromQuery] TimeSpan timeSpan, IMediator mediator, ClaimsPrincipal user, CancellationToken ct) =>
            {
                return Results.Ok();
            }).RequireAuthorization(policy => policy.RequireRole("admin", "user"));

        // -------------------------------------------------------------------------------------------------------------
        // Unlock resource
        app.MapPut(
            "resource/{id}/unlock",
            async (Guid id, IMediator mediator, ClaimsPrincipal user, CancellationToken ct) =>
            {
                return Results.Ok();
            }).RequireAuthorization(policy => policy.RequireRole("admin", "user"));

        // -------------------------------------------------------------------------------------------------------------
        // Withdraw resource
        app.MapPut(
            "resource/{id}",
            async (Guid id, WithdrawResourceCommand command, IMediator mediator, CancellationToken ct) =>
            {
                return Results.Ok();
            }).RequireAuthorization(policy => policy.RequireRole("admin"));

        // -------------------------------------------------------------------------------------------------------------
        // Getting resources
        app.MapGet(
            "resource/{id}",
            async (Guid id, IMediator mediator, CancellationToken ct) =>
            {
                ResourceDto resourceDto = await mediator.Send(new GetResourceQuery { Id = id }, ct);
                return Results.Ok(resourceDto);
            }).WithName("GetResourceById");

        return app;
    }
}
