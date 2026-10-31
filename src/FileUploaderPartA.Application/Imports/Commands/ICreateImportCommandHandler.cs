using FileUploaderPartA.Core.Shared;

namespace FileUploaderPartA.Application.Imports.Commands;

public interface ICreateImportCommandHandler
{
    Task<Result<bool>> HandleAsync(CreateImportCommand request, CancellationToken cancellationToken);
}