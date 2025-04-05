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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportFileAsync([FromForm] CreateImportRequest newImportFile)
    {
        Result<bool> file = await _mediator.Send(new CreateImportCommand(newImportFile));
        return file.IsSuccess
            ? NoContent()
            : BadRequest(file);
    }
}