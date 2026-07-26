namespace FileUploaderPartA.Infrastructure.Interfaces.Services;

public interface IS3Service
{
    Task UploadFileAsync(string bucket, Stream fileStream, string destinyPath);
}