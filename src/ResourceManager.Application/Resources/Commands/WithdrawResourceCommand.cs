using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ResourceManager.Application.Common;
using ResourceManager.Application.Common.Interfaces;
using ResourceManager.Application.DTOs;
using ResourceManager.Domain.Resources;

namespace ResourceManager.Application.Resources.Commands;

public class WithdrawResourceCommand : IRequest<ResourceDto>
{
    public Guid Id { get; set; }
}

public class WithdrawResourceCommandValidator : AbstractValidator<WithdrawResourceCommand>
{
}

public class WithdrawResourceCommandHandler : IRequestHandler<WithdrawResourceCommand, ResourceDto>
{
    private readonly IResourceDbContext _resourceDbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public WithdrawResourceCommandHandler(IResourceDbContext resourceDbContext, IDateTimeProvider dateTimeProvider)
    {
        _resourceDbContext = resourceDbContext;
        _dateTimeProvider = dateTimeProvider;
    }
    public async Task<ResourceDto> Handle(WithdrawResourceCommand command, CancellationToken ct)
    {
        try
        {
            Resource resource = await _resourceDbContext.Resources.FindAsync(command.Id)
                                ?? throw new ApplicationLogicException("There's no resource with this id");

            DateTimeOffset now = _dateTimeProvider.Now;
            resource.Withdraw();

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
