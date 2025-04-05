namespace FileUploaderPartA.Infrastructure.Configurations;

public class AmazonS3Configuration
{
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
}