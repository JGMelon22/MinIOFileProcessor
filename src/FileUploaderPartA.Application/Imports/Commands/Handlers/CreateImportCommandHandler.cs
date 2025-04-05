using FileUploaderPartA.Core.Shared;
using MediatR;

namespace FileUploaderPartA.Application.Imports.Commands.Handlers;

public class CreateImportCommandHandler : IRequestHandler<CreateImportCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(CreateImportCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}