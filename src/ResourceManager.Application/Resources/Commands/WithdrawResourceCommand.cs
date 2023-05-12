using FluentValidation;
using MediatR;

namespace ResourceManager.Application.Resources.Commands;

public class WithdrawResourceCommand : IRequest<Guid>
{
}

public class WithdrawResourceCommandValidator : AbstractValidator<WithdrawResourceCommand>
{
}

public class WithdrawResourceCommandHandler : IRequestHandler<WithdrawResourceCommand, Guid>
{
    public Task<Guid> Handle(WithdrawResourceCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
