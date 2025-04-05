using FileUploaderPartA.Core.Domains.Imports.Entities;
using FileUploaderPartA.Core.Shared;

namespace FileUploaderPartA.Infrastructure.Interfaces.Repository;

public interface IImportRepository
{
    Task<Result<bool>> CreateImportAsync(Import import);
}