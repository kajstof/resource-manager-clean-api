using FluentValidation;
using MediatR;

namespace ResourceManager.Application.Resources.Commands;

public class UnlockResourceCommand : IRequest<Guid>
{
}

public class UnlockResourceCommandValidator : AbstractValidator<UnlockResourceCommand>
{
}

public class UnlockResourceCommandHandler : IRequestHandler<UnlockResourceCommand, Guid>
{
    public Task<Guid> Handle(UnlockResourceCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
