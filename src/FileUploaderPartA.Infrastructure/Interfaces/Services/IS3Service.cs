using Microsoft.AspNetCore.Http;

namespace FileUploaderPartA.Infrastructure.Interfaces.Services;

public interface IS3Service
{
    Task UploadFileAsync(string bucket, IFormFile file, string destinyPath);
}