using FileUploaderPartA.Core.Domains.Imports.Entities;
using FileUploaderPartA.Core.Domains.Imports.Enums;

namespace FileUploaderPartA.Core.Domains.Imports.Mappings;

public static class MappingExtensions
{
    public static Import ToDomain(string s3Path)
        => new Import(
            id: Guid.NewGuid().ToString(),
            s3Path: s3Path,
            status: Status.Pending
        );
}
