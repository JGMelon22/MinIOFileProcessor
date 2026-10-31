using FileUploaderPartA.Application.Imports.Commands;
using FileUploaderPartA.Core.Domains.Imports.Dtos;
using FileUploaderPartA.Core.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FileUploaderPartA.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImportsController(
    ICreateImportCommandHandler handler) : ControllerBase
{
    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportFileAsync(IFormFile newImportFile, CancellationToken cancellationToken)
    {
        FileData fileData = new(
            Content: newImportFile.OpenReadStream(),
            FileName: newImportFile.FileName,
            ContentType: newImportFile.ContentType,
            Length: newImportFile.Length
        );

        CreateImportRequest request = new(fileData);
        CreateImportCommand createImportCommand = new CreateImportCommand(request);

        Result<bool> file = await handler.HandleAsync(createImportCommand, cancellationToken);
        return file.IsSuccess
            ? Created()
            : BadRequest(file);
    }
}