using Dapper;
using FileUploaderPartA.Core.Domains.Imports.Entities;
using FileUploaderPartA.Infrastructure.Data;
using FileUploaderPartA.Infrastructure.Interfaces.Repository;
using Microsoft.Extensions.Logging;
using System.Data;

namespace FileUploaderPartA.Infrastructure.Repositories;

public class ImportRepository : IImportRepository
{
    private readonly DapperDbContext _dbContext;
    private readonly ILogger<ImportRepository> _logger;

    public ImportRepository(DapperDbContext dbContext, ILogger<ImportRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(Import import)
    {
        try
        {
            _logger.LogInformation("{Repository}.{Method} - Start: Creating import {@Import}",
                GetType().Name, nameof(CreateAsync), import);

            const string sql = """
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
                               );
                               """;

            var parameters = new
            {
                import.Id,
                import.S3Path,
                Status = nameof(import.Status.Pending),
                import.CreatedAt
            };

            using var connection = _dbContext.CreateConnection();

            var result = await connection.ExecuteAsync(sql, parameters);

            _logger.LogInformation("{Repository}.{Method} - Success: Import created {ImportId}",
                GetType().Name, nameof(CreateAsync), import.Id);

            return result == 1;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Repository}.{Method} - Error creating import {@Import}",
                GetType().Name, nameof(CreateAsync), import);

            throw;
        }
    }
}