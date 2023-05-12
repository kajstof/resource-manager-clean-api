using FluentValidation;
using MediatR;
using ResourceManager.Application.Common.Interfaces;
using ResourceManager.Application.DTOs;
using ResourceManager.Domain.Resources;

namespace ResourceManager.Application.Resources.Commands;

public class CreateResourceCommand : IRequest<ResourceDto>
{
    public string Name { get; set; } = null!;
}

public class CreateResourceCommandValidator : AbstractValidator<CreateResourceCommand>
{
}

public class CreateResourceCommandHandler : IRequestHandler<CreateResourceCommand, ResourceDto>
{
    private readonly IResourceDbContext _resourceDbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateResourceCommandHandler(IResourceDbContext resourceDbContext, IDateTimeProvider dateTimeProvider)
    {
        _resourceDbContext = resourceDbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ResourceDto> Handle(CreateResourceCommand command, CancellationToken ct)
    {
        Resource resource = Resource.CreateNew(Guid.NewGuid(), command.Name);
        await _resourceDbContext.Resources.AddAsync(resource, ct);
        await _resourceDbContext.SaveChangesAsync(ct);
        return ResourceDto.FromResource(resource, _dateTimeProvider.Now);
    }
}
