using FileUploaderPartA.Core.Domains.Imports.Entities;
using FileUploaderPartA.Core.Domains.Imports.Enums;

namespace FileUploaderPartA.Core.Domains.Imports.Mappings;

public static class MappingExtensions
{
    public static Import ToDomain(string s3Path)
    {
        return new Import(
            Guid.NewGuid().ToString(),
            s3Path,
            Status.Pending
        );
    }
}