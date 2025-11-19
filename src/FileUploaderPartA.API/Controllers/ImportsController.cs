using FileUploaderPartA.Application.Imports.Commands;
using FileUploaderPartA.Core.Domains.Imports.Dtos;
using FileUploaderPartA.Core.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FileUploaderPartA.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportFileAsync(IFormFile newImportFile)
    {
        FileData fileData = new(
           Content: newImportFile.OpenReadStream(),
           FileName: newImportFile.FileName,
           ContentType: newImportFile.ContentType,
           Length: newImportFile.Length
       );

        CreateImportRequest request = new(fileData);

        Result<bool> file = await _mediator.Send(new CreateImportCommand(request));
        return file.IsSuccess
            ? NoContent()
            : BadRequest(file);
    }
}