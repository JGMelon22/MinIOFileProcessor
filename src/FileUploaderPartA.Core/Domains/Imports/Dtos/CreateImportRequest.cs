using FileUploaderPartA.Core.Shared;
using FileUploaderPartA.Core.Shared.Validations.FileValidations;
using System.ComponentModel.DataAnnotations;

namespace FileUploaderPartA.Core.Domains.Imports.Dtos;

public record CreateImportRequest(
    [Display(Name = "CSV File to be uploaded")]
    [Required]
    [CsvValidations]
    FileData CsvFile
);