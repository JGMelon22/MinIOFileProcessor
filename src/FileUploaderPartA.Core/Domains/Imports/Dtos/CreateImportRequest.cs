using FileUploaderPartA.Core.Shared.Validations.FileValidations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FileUploaderPartA.Core.Domains.Imports.Dtos;

public record CreateImportRequest
(
    [Display(Name = "CSV File to be uploaded")]
    [Required]
    [CsvValidations]
    IFormFile CsvFile
);
