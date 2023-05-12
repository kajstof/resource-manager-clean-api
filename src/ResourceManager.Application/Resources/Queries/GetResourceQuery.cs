using FluentValidation;
using MediatR;
using ResourceManager.Application.Common;
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
    private readonly IDateTimeProvider _dateTimeProvider;

    public GetResourceQueryHandler(IResourceDbContext resourceDbContext, IDateTimeProvider dateTimeProvider)
    {
        _resourceDbContext = resourceDbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ResourceDto> Handle(GetResourceQuery query, CancellationToken cancellationToken)
    {
        Resource resource = await _resourceDbContext.Resources.FindAsync(query.Id)
                            ?? throw new ApplicationLogicException("There's no resource with this id");

        DateTimeOffset now = _dateTimeProvider.Now;
        return new ResourceDto(
            resource.Id, resource.Name, resource.IsLockedAtTheMoment(now), resource.LockedTo(now));
    }
}
