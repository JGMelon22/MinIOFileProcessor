using FileUploaderPartA.Core.Domains.Imports.Enums;

namespace FileUploaderPartA.Core.Domains.Imports.Entities;

public class Import
{
    public Import() { }

    public Import(string id, string s3Path, Status status)
    {
        Id = id;
        S3Path = s3Path;
        Status = status;
        CreatedAt = DateTime.UtcNow;
    }

    public string Id { get; set; } = string.Empty;
    public string S3Path { get; set; } = string.Empty;
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
