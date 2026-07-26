using FileUploaderPartA.Application.Imports.Commands;
using FileUploaderPartA.Core.Domains.Imports.Dtos;
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportFileAsync([FromForm] CreateImportRequest newImportFile)
    {
        var file = await _mediator.Send(new CreateImportCommand(newImportFile));
        return file.IsSuccess
            ? Created()
            : BadRequest(file);
    }
}