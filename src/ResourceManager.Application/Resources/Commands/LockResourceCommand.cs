using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ResourceManager.Application.Common;
using ResourceManager.Application.Common.Interfaces;
using ResourceManager.Application.DTOs;
using ResourceManager.Domain.Resources;

namespace ResourceManager.Application.Resources.Commands;

public class LockResourceCommand : IRequest<ResourceDto>
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public DateTimeOffset DateTimeOffset { get; set; }
}

public class LockResourceCommandValidator : AbstractValidator<LockResourceCommand>
{
}

public class LockResourceCommandHandler : IRequestHandler<LockResourceCommand, ResourceDto>
{
    private readonly IResourceDbContext _resourceDbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public LockResourceCommandHandler(IResourceDbContext resourceDbContext, IDateTimeProvider dateTimeProvider)
    {
        _resourceDbContext = resourceDbContext;
        _dateTimeProvider = dateTimeProvider;
    }
    public async Task<ResourceDto> Handle(LockResourceCommand command, CancellationToken ct)
    {
        try
        {
            Resource resource = await _resourceDbContext.Resources.FindAsync(command.Id)
                                ?? throw new ApplicationLogicException("There's no resource with this id");

            DateTimeOffset now = _dateTimeProvider.Now;
            resource.LockTemporary(command.Username, now, command.DateTimeOffset);

            _resourceDbContext.Resources.Update(resource);
            await _resourceDbContext.SaveChangesAsync(ct);

            return ResourceDto.FromResource(resource, now);
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new ConcurrencyException(e);
        }
    }
}
