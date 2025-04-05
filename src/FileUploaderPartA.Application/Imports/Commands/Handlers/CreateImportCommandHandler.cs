using FileUploaderPartA.Core.Shared;
using FileUploaderPartA.Infrastructure.Configurations;
using FileUploaderPartA.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace FileUploaderPartA.Application.Imports.Commands.Handlers;

public class CreateImportCommandHandler : IRequestHandler<CreateImportCommand, Result<bool>>
{
    private readonly S3Service _s3Service;
    private readonly FileUploadConfiguration _uploadConfiguration;

    public CreateImportCommandHandler(S3Service s3Service, IOptions<FileUploadConfiguration> uploadConfigurationOptions)
    {
        _s3Service = s3Service;
        _uploadConfiguration = uploadConfigurationOptions.Value;
    }

    public async Task<Result<bool>> Handle(CreateImportCommand request, CancellationToken cancellationToken)
    {
        IFormFile file = request.request.CsvFile;

        string fileName = Path.GetFileName(file.FileName);
        string uniqueName = $"{Guid.NewGuid()}_{fileName}";
        string s3Key = uniqueName;

        bool uploadSuccess = await _s3Service.UploadFileAsync(
            bucket: _uploadConfiguration.BucketName,
            file: file,
            destinyPath: s3Key
        );

        if (!uploadSuccess)
            return Result<bool>.Failure("Failed to upload the file to MinIO.");

        string s3Url = $"{_uploadConfiguration.DNS}/{_uploadConfiguration.BucketName}/{s3Key}";

        // Import import = request.request.ToDomain(s3Url);

        return Result<bool>.Success(true);

    }
}