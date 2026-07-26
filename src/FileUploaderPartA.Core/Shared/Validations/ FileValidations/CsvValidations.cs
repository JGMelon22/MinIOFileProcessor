using System.ComponentModel.DataAnnotations;

namespace FileUploaderPartA.Core.Shared.Validations.FileValidations;

public class CsvValidations : ValidationAttribute
{
    private const long MaxFileSizeInBytes = 2 * 1024 * 1024; // 2MB
    private readonly string[] ValidMimeTypes = { "text/csv", "application/vnd.ms-excel" };

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return new ValidationResult("A CSV file is required.");

        if (value is not FileData file)
            return new ValidationResult($"The input must be a {nameof(FileData)}.");

        if (file.Content == null)
            return new ValidationResult("File content cannot be null.");

        if (file.Length <= 0)
            return new ValidationResult("File cannot be empty.");

        if (file.Length > MaxFileSizeInBytes)
            return new ValidationResult(
                    $"The file size exceeds the maximum allowed size of {MaxFileSizeInBytes / (1024 * 1024)}MB.");

        if (string.IsNullOrEmpty(file.ContentType) || !ValidMimeTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
            return new ValidationResult(
                    $"File must be one of the following types: {string.Join(", ", ValidMimeTypes)}");

        return ValidationResult.Success;
    }
}