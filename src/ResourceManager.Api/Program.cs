using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResourceManager.Api.Extensions;
using ResourceManager.Application.DependencyInjection;
using ResourceManager.Application.DTOs;
using ResourceManager.Application.Resources.Commands;
using ResourceManager.Application.Resources.Queries;
using ResourceManager.Infrastructure.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuthentication();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// ---------------------------------------------------------------------------------------------------------------------
// Create resource
app.MapPost(
    "resource",
    async (CreateResourceCommand command, IMediator mediator, CancellationToken ct) =>
    {
        ResourceDto resourceDto = await mediator.Send(command, ct);
        return Results.CreatedAtRoute("GetResourceById", new { id = resourceDto.Id }, resourceDto);
    }).RequireAuthorization(policy => policy.RequireRole("admin"));

// ---------------------------------------------------------------------------------------------------------------------
// Locking resource
app.MapPut("resource/{id}/lock",
    async (Guid id, [FromQuery] DateTimeOffset? validTo, IMediator mediator, ClaimsPrincipal user, CancellationToken ct) =>
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

// ---------------------------------------------------------------------------------------------------------------------
// Unlock resource
app.MapPut("resource/{id}/unlock", async (Guid id, IMediator mediator, ClaimsPrincipal user, CancellationToken ct) =>
{
    UnlockResourceCommand command = new()
    {
        Id = id,
        Username = user.Identity?.Name ?? throw new InvalidOperationException()
    };
    ResourceDto resourceDto = await mediator.Send(command, ct);
    return Results.CreatedAtRoute("GetResourceById", new { id = resourceDto.Id }, resourceDto);
}).RequireAuthorization(policy => policy.RequireRole("admin", "user"));

// ---------------------------------------------------------------------------------------------------------------------
// Withdraw resource
app.MapPut("resource/{id}/withdraw", async (Guid id, IMediator mediator, CancellationToken ct) =>
{
    ResourceDto resourceDto = await mediator.Send(new WithdrawResourceCommand { Id = id }, ct);
    return Results.CreatedAtRoute("GetResourceById", new { id = resourceDto.Id }, resourceDto);
}).RequireAuthorization(policy => policy.RequireRole("admin"));

// ---------------------------------------------------------------------------------------------------------------------
// Getting resources
app.MapGet("resource/{id}", async (Guid id, IMediator mediator, CancellationToken ct) =>
{
    ResourceDto resourceDto = await mediator.Send(new GetResourceQuery { Id = id }, ct);
    return Results.Ok(resourceDto);
}).WithName("GetResourceById");

// ---------------------------------------------------------------------------------------------------------------------

app.Run();
