using FluentValidation;
using MediatR;
using ResourceManager.Application.Common;
using ResourceManager.Application.Common.Interfaces;
using ResourceManager.Application.DTOs;
using ResourceManager.Domain.Resources;

namespace ResourceManager.Application.Resources.Commands;

public class UnlockResourceCommand : IRequest<ResourceDto>
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;
}

public class UnlockResourceCommandValidator : AbstractValidator<UnlockResourceCommand>
{
}

public class UnlockResourceCommandHandler : IRequestHandler<UnlockResourceCommand, ResourceDto>
{
    private readonly IResourceDbContext _resourceDbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UnlockResourceCommandHandler(IResourceDbContext resourceDbContext, IDateTimeProvider dateTimeProvider)
    {
        _resourceDbContext = resourceDbContext;
        _dateTimeProvider = dateTimeProvider;
    }
    public async Task<ResourceDto> Handle(UnlockResourceCommand command, CancellationToken ct)
    {
        Resource resource = await _resourceDbContext.Resources.FindAsync(command.Id)
                            ?? throw new ApplicationLogicException("There's no resource with this id");

        DateTimeOffset now = _dateTimeProvider.Now;
        resource.Unlock(command.Username, now);

        _resourceDbContext.Resources.Update(resource);
        await _resourceDbContext.SaveChangesAsync(ct);

        return ResourceDto.FromResource(resource, now);
    }
}
