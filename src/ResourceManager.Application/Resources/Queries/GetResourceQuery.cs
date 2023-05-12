using FluentValidation;
using MediatR;
using ResourceManager.Application.Common.Interfaces;
using ResourceManager.Application.DTOs;
using ResourceManager.Domain.Resources;

namespace ResourceManager.Application.Resources.Queries;

public class GetResourceQuery : IRequest<ResourceDto>
{
    public Guid Id { get; set; }
}

public class GetResourceQueryValidator : AbstractValidator<GetResourceQuery>
{
}

public class GetResourceQueryHandler : IRequestHandler<GetResourceQuery, ResourceDto>
{
    private readonly IResourceDbContext _resourceDbContext;
    public GetResourceQueryHandler(IResourceDbContext resourceDbContext)
    {
        _resourceDbContext = resourceDbContext;
    }

    public async Task<ResourceDto> Handle(GetResourceQuery query, CancellationToken cancellationToken)
    {
        Resource resource = await _resourceDbContext.Resources.FindAsync(query.Id)
                            ?? throw new InvalidOperationException("There's no resource with this id");

        DateTimeOffset now = DateTimeOffset.Now;
        return new ResourceDto(
            resource.Id, resource.Name, resource.IsLockedAtTheMoment(now), resource.LockedTo(now));
    }
}
