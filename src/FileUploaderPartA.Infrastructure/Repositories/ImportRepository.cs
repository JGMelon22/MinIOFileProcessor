using System.Data;
using Dapper;
using FileUploaderPartA.Core.Domains.Imports.Entities;
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
            _logger.LogInformation($"{GetType()}.{nameof(CreateAsync)} - Start: Attempting to insert import with Id {import.Id}.");

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

            // string status = nameof(import.Status);
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

                _logger.LogInformation($"{GetType()}.{nameof(CreateAsync)} - Success: Import with Id {import.Id} inserted successfully.");

                return result == 1;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{GetType()}.{nameof(CreateAsync)} - Error: An error occurred while inserting import with Id {import.Id}.");
            return false;
        }
    }
}