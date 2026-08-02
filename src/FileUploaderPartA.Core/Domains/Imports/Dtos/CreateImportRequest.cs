using FileUploaderPartA.Core.Shared;
using System.ComponentModel.DataAnnotations;
using FileUploaderPartA.Core.Shared.Validations._FileValidations;

namespace FileUploaderPartA.Core.Domains.Imports.Dtos;

public record CreateImportRequest(
    [Display(Name = "CSV File to be uploaded")]
    [Required]
    [CsvValidations]
    FileData CsvFile
);