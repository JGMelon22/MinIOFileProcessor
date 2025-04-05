using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FileUploaderPartA.Core.Shared.Validations.FileValidations;

public class CsvValidations : ValidationAttribute
{
    private const long MaxFileSizeInBytes = 2 * 1024 * 1024; // 2MB
    private readonly string[] ValidMimeTypes = { "text/csv" };

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return new ValidationResult("A CSV file is required.");

        if (value is IFormFile file)
        {
            if (file.Length > MaxFileSizeInBytes)
                return new ValidationResult($"The file size exceeds the maximum allowed size of {MaxFileSizeInBytes / 1024}KB.");

            if (!ValidMimeTypes.Contains(file.ContentType))
                return new ValidationResult($"File must be one of the following types: {string.Join(", ", ValidMimeTypes)}");

            return ValidationResult.Success;
        }

        return new ValidationResult($"The input must be an {nameof(IFormFile)} or {nameof(Stream)}.");
    }
}
