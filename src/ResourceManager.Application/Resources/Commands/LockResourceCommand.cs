using FluentValidation;
using MediatR;

namespace ResourceManager.Application.Resources.Commands;

public class LockResourceCommand : IRequest<Guid>
{
}

public class LockResourceCommandValidator : AbstractValidator<LockResourceCommand>
{
}

public class LockResourceCommandHandler : IRequestHandler<LockResourceCommand, Guid>
{
    public Task<Guid> Handle(LockResourceCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
