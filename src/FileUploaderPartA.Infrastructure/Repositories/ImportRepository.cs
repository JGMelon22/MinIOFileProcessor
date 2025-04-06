using System.Data;
using Dapper;
using FileUploaderPartA.Core.Domains.Imports.Entities;
using FileUploaderPartA.Core.Domains.Imports.Enums;
using FileUploaderPartA.Core.Shared;
using FileUploaderPartA.Infrastructure.Data;
using FileUploaderPartA.Infrastructure.Interfaces.Repository;
using Microsoft.Extensions.Logging;

namespace FileUploaderPartA.Infrastructure.Repositories;

public class ImportRepository : IImportRepository
{
    private readonly DapperDbContext _dbContext;
    private ILogger<ImportRepository> _logger;

    public ImportRepository(DapperDbContext dbContext, ILogger<ImportRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(Import import)
    {
        try
        {
            _logger.LogInformation("{Repository}.{Method} - Start: Attempting to insert import. Id: {ImportId}",
    GetType().Name, nameof(CreateAsync), import.Id);

            const string sql = @"
            INSERT INTO imports
            (
               id,
               s3_path,
               status,
               created_at
            )
            VALUES
            (
               @Id,
               @S3Path,
               @Status,
               @CreatedAt
            );";

            string status = import.Status.ToString();

            var parameters = new
            {
                import.Id,
                import.S3Path,
                status,
                import.CreatedAt
            };

            using (IDbConnection connection = _dbContext.CreateConnection())
            {
                int result = await connection.ExecuteAsync(sql, parameters);

                _logger.LogInformation("{Repository}.{Method} - Success: Import inserted. Id: {ImportId}",
                                GetType().Name,
                                nameof(CreateAsync),
                                import.Id);

                return result == 1;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                       "{Repository}.{Method} - Error inserting import. Id: {ImportId}",
                       GetType().Name,
                       nameof(CreateAsync),
                       import.Id);

            return false;
        }
    }
}
