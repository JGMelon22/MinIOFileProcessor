namespace FileUploaderPartA.Core.Shared;

public record FileData(
    Stream Content,
    string FileName,
    string ContentType,
    long Length
);