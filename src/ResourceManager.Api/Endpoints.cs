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
            async (Guid id, [FromQuery] DateTimeOffset? validTo, IMediator mediator, ClaimsPrincipal user,
                CancellationToken ct) =>
            {
                LockResourceCommand command = new()
                {
                    Id = id,
                    DateTimeOffset = validTo ?? DateTimeOffset.MaxValue,
                    Username = user.Identity?.Name ?? throw new InvalidOperationException()
                };
                ResourceDto resourceDto = await mediator.Send(command, ct);
                return Results.CreatedAtRoute("GetResourceById", new { id = resourceDto.Id }, resourceDto);
            }).RequireAuthorization(policy => policy.RequireRole("admin", "user"));

        // -------------------------------------------------------------------------------------------------------------
        // Unlock resource
        app.MapPut(
            "resource/{id}/unlock",
            async (Guid id, IMediator mediator, ClaimsPrincipal user, CancellationToken ct) =>
            {
                UnlockResourceCommand command = new()
                {
                    Id = id,
                    Username = user.Identity?.Name ?? throw new InvalidOperationException()
                };
                ResourceDto resourceDto = await mediator.Send(command, ct);
                return Results.CreatedAtRoute("GetResourceById", new { id = resourceDto.Id }, resourceDto);
            }).RequireAuthorization(policy => policy.RequireRole("admin", "user"));

        // -------------------------------------------------------------------------------------------------------------
        // Withdraw resource
        app.MapPut(
            "resource/{id}/withdraw",
            async (Guid id, IMediator mediator, CancellationToken ct) =>
            {
                ResourceDto resourceDto = await mediator.Send(new WithdrawResourceCommand { Id = id }, ct);
                return Results.CreatedAtRoute("GetResourceById", new { id = resourceDto.Id }, resourceDto);
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
