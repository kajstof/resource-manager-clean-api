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

    public async Task<ResourceDto> Handle(GetResourceQuery query, CancellationToken ct)
    {
        Resource resource = await _resourceDbContext.Resources.FindAsync(query.Id)
                            ?? throw new ApplicationLogicException("There's no resource with this id");
        await _resourceDbContext.Resources.AddAsync(resource, ct);
        return ResourceDto.FromResource(resource, _dateTimeProvider.Now);
    }
}
