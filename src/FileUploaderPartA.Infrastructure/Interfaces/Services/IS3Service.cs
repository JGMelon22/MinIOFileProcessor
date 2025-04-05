using Microsoft.AspNetCore.Http;

namespace FileUploaderPartA.Infrastructure.Interfaces.Services;

public interface IS3Service
{
    Task<bool> UploadFileAsync(string bucket, IFormFile file, string destinyPath);
}