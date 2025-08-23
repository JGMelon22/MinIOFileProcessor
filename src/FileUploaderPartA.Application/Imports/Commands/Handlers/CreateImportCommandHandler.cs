using FileUploaderPartA.Core.Domains.Imports.Entities;
using FileUploaderPartA.Core.Domains.Imports.Mappings;
using FileUploaderPartA.Core.Shared;
using FileUploaderPartA.Infrastructure.Configurations;
using FileUploaderPartA.Infrastructure.Interfaces.Repository;
using FileUploaderPartA.Infrastructure.Interfaces.Services;
using FileUploaderPartA.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace FileUploaderPartA.Application.Imports.Commands.Handlers;

public class CreateImportCommandHandler : IRequestHandler<CreateImportCommand, Result<bool>>
{
    private readonly S3Service _s3Service;
    private readonly IImportRepository _importRepository;
    private readonly FileUploadConfiguration _uploadConfiguration;
    private readonly IKafkaProducerService _kafkaProducerService;
    private readonly string _importsTopic;

    public CreateImportCommandHandler(
        S3Service s3Service,
        IImportRepository importRepository,
        IOptions<FileUploadConfiguration> uploadConfigurationOptions,
        IOptions<KafkaConfiguration> kafkaConfigOptions,
        IKafkaProducerService kafkaProducerService
    )
    {
        _s3Service = s3Service;
        _importRepository = importRepository;
        _uploadConfiguration = uploadConfigurationOptions.Value;
        _kafkaProducerService = kafkaProducerService;
        _importsTopic = kafkaConfigOptions.Value.ImportsTopic;
    }

    public async Task<Result<bool>> Handle(CreateImportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            IFormFile file = request.request.CsvFile;

            string fileName = Path.GetFileName(file.FileName);
            string uniqueName = $"{Guid.NewGuid()}_{fileName}";
            string s3Key = uniqueName;

            await _s3Service.UploadFileAsync(
                            bucket: _uploadConfiguration.BucketName,
                            file: file,
                            destinyPath: s3Key
                        );

            string s3Url = $"{_uploadConfiguration.DNS}/{_uploadConfiguration.BucketName}/{s3Key}";

            Import import = MappingExtensions.ToDomain(s3Url);

            await _importRepository.CreateAsync(import);
            await _kafkaProducerService.ProduceAsync(import.Id, import, _importsTopic);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"An unexpected error occurred: {ex.Message}");
        }
    }
}