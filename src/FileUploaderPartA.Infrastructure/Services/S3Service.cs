using Amazon.S3;
using Amazon.S3.Transfer;
using FileUploaderPartA.Infrastructure.Configurations;
using FileUploaderPartA.Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FileUploaderPartA.Infrastructure.Services;

public class S3Service : IS3Service
{
    private readonly ILogger<S3Service> _logger;
    private readonly IAmazonS3 _s3Client;

    public S3Service(IOptions<AmazonS3Configuration> options, ILogger<S3Service> logger)
    {
        _s3Client = new AmazonS3Client(
            options.Value.AccessKey,
            options.Value.SecretKey,
            new AmazonS3Config
            {
                ServiceURL = options.Value.ServiceURL,
                ForcePathStyle = options.Value.ForcePathStyle
            }
        );
        _logger = logger;
    }

    public async Task UploadFileAsync(string bucket, Stream fileStream, string destinyPath)
    {
        try
        {
            _logger.LogInformation("Starting file upload to S3. Bucket: {Bucket}, Key: {Key}", bucket, destinyPath);

            TransferUtility fileTransferUtility = new(_s3Client);

            TransferUtilityUploadRequest uploadRequest = new()
            {
                InputStream = fileStream,
                Key = destinyPath,
                BucketName = bucket
            };

            await fileTransferUtility.UploadAsync(uploadRequest);

            _logger.LogInformation("File uploaded successfully to S3. Bucket: {Bucket}, Key: {Key}", bucket,
                destinyPath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload file to S3. Bucket: {Bucket}, Key: {Key}", bucket, destinyPath);
            throw;
        }
    }
}