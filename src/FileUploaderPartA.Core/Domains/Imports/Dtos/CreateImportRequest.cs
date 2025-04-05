using System.ComponentModel.DataAnnotations;
using FileUploaderPartA.Core.Shared.Validations.FileValidations;
using Microsoft.AspNetCore.Http;

namespace FileUploaderPartA.Core.Domains.Imports.Dtos;

public record CreateImportRequest
(
    [Display(Name = "CSV File to be uploaded")]
    [Required]
    [CsvValidations]
    IFormFile CsvFile
);
