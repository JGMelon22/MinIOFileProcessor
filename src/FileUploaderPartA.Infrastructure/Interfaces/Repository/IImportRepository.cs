using FileUploaderPartA.Core.Domains.Imports.Entities;

namespace FileUploaderPartA.Infrastructure.Interfaces.Repository;

public interface IImportRepository
{
    Task<bool> CreateAsync(Import import);
}