using FileUploaderPartA.API.Controllers;
using FileUploaderPartA.Application.Imports.Commands;
using FileUploaderPartA.Core.Domains.Imports.Dtos;
using FileUploaderPartA.Core.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;

namespace FileUploaderPartA.Api.UnitTests.Controllers.Imports;

public class ImportsControllerTests
{
    [Fact]
    public async Task Should_ReturnCreatedResult_When_ImportFileSucceeds()
    {
        // Arrange 
        Mock<IMediator> mediator = new();
        CreateImportRequest importRequest = new(new Mock<IFormFile>().Object);
        var controller = new ImportsController(mediator.Object);

        mediator.Setup(m => m.Send(It.IsAny<CreateImportCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<bool>.Success(true));

        // Act
        var result = await controller.ImportFileAsync(importRequest);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<CreatedResult>();
    }

    [Fact]
    public async Task Should_ReturnBadRequest_When_ImportFileFails()
    {
        // Arrange 
        Mock<IMediator> mediator = new();
        CreateImportRequest importRequest = new(new Mock<IFormFile>().Object);
        var controller = new ImportsController(mediator.Object);

        mediator.Setup(m => m.Send(It.IsAny<CreateImportCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<bool>.Failure("Import failed"));

        // Act
        var result = await controller.ImportFileAsync(importRequest);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<BadRequestObjectResult>();
    }
}