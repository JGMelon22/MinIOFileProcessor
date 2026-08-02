using FileUploaderPartA.API.Controllers;
using FileUploaderPartA.Application.Imports.Commands;
using FileUploaderPartA.Core.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDevPack.SimpleMediator;
using NSubstitute;

namespace FileUploaderPartA.Api.UnitTests.Controllers.Imports;

[TestFixture]
public class ImportsControllerTests
{
    private IMediator _mediator;
    private ImportsController _controller;

    [SetUp]
    public void Setup()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new ImportsController(_mediator);
    }

    [Test]
    public async Task Should_ReturnBadRequest_When_FileIsLargerThanMaximumAllowed()
    {
        // Arrange
        _mediator
            .Send(Arg.Any<CreateImportCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<bool>.Failure("File exceeds the maximum allowed size."));

        IFormFile invalidFile = Substitute.For<IFormFile>();
        invalidFile.Length.Returns(10 * 1024 * 1024); // 10 MB
        invalidFile.ContentType.Returns("text/csv");
        invalidFile.FileName.Returns("test.csv");
        invalidFile.OpenReadStream().Returns(new MemoryStream());

        // Act
        var result = await _controller.ImportFileAsync(invalidFile);

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Should_ReturnBadRequest_When_FileMimeTypeIsInvalid()
    {
        // Arrange
        _mediator
            .Send(Arg.Any<CreateImportCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<bool>.Failure("Invalid file mime type."));

        IFormFile invalidFile = Substitute.For<IFormFile>();
        invalidFile.Length.Returns(1024);
        invalidFile.ContentType.Returns("application/pdf");
        invalidFile.FileName.Returns("test.pdf");
        invalidFile.OpenReadStream().Returns(new MemoryStream());

        // Act
        var result = await _controller.ImportFileAsync(invalidFile);

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Should_ReturnCreated_When_FileIsValid()
    {
        // Arrange
        _mediator
            .Send(Arg.Any<CreateImportCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<bool>.Success(true));

        IFormFile validFile = Substitute.For<IFormFile>();
        validFile.Length.Returns(1024);
        validFile.ContentType.Returns("text/csv");
        validFile.FileName.Returns("valid.csv");
        validFile.OpenReadStream().Returns(new MemoryStream());

        // Act
        var result = await _controller.ImportFileAsync(validFile);

        // Assert
        Assert.That(result, Is.TypeOf<CreatedResult>());
    }
}