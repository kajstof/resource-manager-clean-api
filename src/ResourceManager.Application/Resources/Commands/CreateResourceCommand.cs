using FluentValidation;
using MediatR;

namespace ResourceManager.Application.Resources.Commands;

public class CreateResourceCommand : IRequest<Guid>
{
}

public class CreateResourceCommandValidator : AbstractValidator<CreateResourceCommand>
{
}

public class CreateResourceCommandHandler : IRequestHandler<CreateResourceCommand, Guid>
{
    public Task<Guid> Handle(CreateResourceCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
