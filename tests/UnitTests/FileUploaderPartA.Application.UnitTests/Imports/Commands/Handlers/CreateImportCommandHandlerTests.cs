using System.Text;
using FileUploaderPartA.Application.Imports.Commands;
using FileUploaderPartA.Application.Imports.Commands.Handlers;
using FileUploaderPartA.Core.Domains.Imports.Dtos;
using FileUploaderPartA.Core.Domains.Imports.Entities;
using FileUploaderPartA.Infrastructure.Configurations;
using FileUploaderPartA.Infrastructure.Interfaces.Repository;
using FileUploaderPartA.Infrastructure.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;

namespace FileUploaderPartA.Application.UnitTests.Imports.Commands.Handlers;

public class CreateImportCommandHandlerTests
{
    [Fact]
    public async Task Should_ReturnSuccess_When_FileUploadedAndImportCreatedAndKafkaProduced()
    {
        // Arrange
        Mock<IS3Service> s3Service = new();
        Mock<IKafkaProducerService> kafkaProducerService = new();
        Mock<IImportRepository> importRepository = new();
        Mock<IOptions<FileUploadConfiguration>> uploadConfigurationOptions = new();
        FileUploadConfiguration uploadConfiguration = new() { BucketName = "test-bucket", DNS = "http://minio:0000" };
        Mock<IOptions<KafkaConfiguration>> kafkaConfigOptions = new();
        KafkaConfiguration kafkaConfiguration = new() { Endpoint = "kafka:00000", ImportsTopic = "some-kafka-topic" };

        s3Service.Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        importRepository.Setup(x => x.CreateAsync(It.IsAny<Import>()))
            .ReturnsAsync(true);

        kafkaProducerService.Setup(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<Import>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        uploadConfigurationOptions.Setup(x => x.Value).Returns(uploadConfiguration);

        kafkaConfigOptions.Setup(x => x.Value).Returns(kafkaConfiguration);

        var filebytes = Encoding.UTF8.GetBytes("sample-data");
        IFormFile file = new FormFile(new MemoryStream(filebytes), 0, filebytes.Length, "Data", "sample-data.csv")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/csv"
        };

        CreateImportRequest importRequest = new(file);

        CreateImportCommand command = new(importRequest);

        CreateImportCommandHandler handler = new(s3Service.Object, importRepository.Object,
            uploadConfigurationOptions.Object, kafkaConfigOptions.Object, kafkaProducerService.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldBeTrue();
        result.Message.ShouldBe("Success");

        importRepository.Verify(x => x.CreateAsync(It.IsAny<Import>()), Times.Once);
        kafkaProducerService.Verify(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<Import>(), It.IsAny<string>()),
            Times.Once);
        s3Service.Verify(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>(), It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task Should_ReturnFailure_When_S3ServiceThrowsException()
    {
        // Arrange
        Mock<IS3Service> s3Service = new();
        Mock<IKafkaProducerService> kafkaProducerService = new();
        Mock<IImportRepository> importRepository = new();
        Mock<IOptions<FileUploadConfiguration>> uploadConfigurationOptions = new();
        FileUploadConfiguration uploadConfiguration = new() { BucketName = "test-bucket", DNS = "http://minio:0000" };
        Mock<IOptions<KafkaConfiguration>> kafkaConfigOptions = new();
        KafkaConfiguration kafkaConfiguration = new() { Endpoint = "kafka:00000", ImportsTopic = "some-kafka-topic" };

        s3Service.Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("S3 error"));

        importRepository.Setup(x => x.CreateAsync(It.IsAny<Import>()))
            .ReturnsAsync(true);

        kafkaProducerService.Setup(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<Import>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        uploadConfigurationOptions.Setup(x => x.Value).Returns(uploadConfiguration);

        kafkaConfigOptions.Setup(x => x.Value).Returns(kafkaConfiguration);

        var filebytes = Encoding.UTF8.GetBytes("sample-data");
        IFormFile file = new FormFile(new MemoryStream(filebytes), 0, filebytes.Length, "Data", "sample-data.csv")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/csv"
        };

        CreateImportRequest importRequest = new(file);

        CreateImportCommand command = new(importRequest);

        CreateImportCommandHandler handler = new(s3Service.Object, importRepository.Object,
            uploadConfigurationOptions.Object, kafkaConfigOptions.Object, kafkaProducerService.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Message.ShouldContain("S3 error");

        importRepository.Verify(x => x.CreateAsync(It.IsAny<Import>()), Times.Never);
        kafkaProducerService.Verify(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<Import>(), It.IsAny<string>()),
            Times.Never);
        s3Service.Verify(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>(), It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task Should_ReturnFailure_When_KafkaProducerThrowsException()
    {
        // Arrange
        Mock<IS3Service> s3Service = new();
        Mock<IKafkaProducerService> kafkaProducerService = new();
        Mock<IImportRepository> importRepository = new();
        Mock<IOptions<FileUploadConfiguration>> uploadConfigurationOptions = new();
        FileUploadConfiguration uploadConfiguration = new() { BucketName = "test-bucket", DNS = "http://minio:0000" };
        Mock<IOptions<KafkaConfiguration>> kafkaConfigOptions = new();
        KafkaConfiguration kafkaConfiguration = new() { Endpoint = "kafka:00000", ImportsTopic = "some-kafka-topic" };

        s3Service.Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        importRepository.Setup(x => x.CreateAsync(It.IsAny<Import>()))
            .ReturnsAsync(true);

        kafkaProducerService.Setup(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<Import>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("Kafka error"));


        uploadConfigurationOptions.Setup(x => x.Value).Returns(uploadConfiguration);

        kafkaConfigOptions.Setup(x => x.Value).Returns(kafkaConfiguration);

        var filebytes = Encoding.UTF8.GetBytes("sample-data");
        IFormFile file = new FormFile(new MemoryStream(filebytes), 0, filebytes.Length, "Data", "sample-data.csv")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/csv"
        };

        CreateImportRequest importRequest = new(file);

        CreateImportCommand command = new(importRequest);

        CreateImportCommandHandler handler = new(s3Service.Object, importRepository.Object,
            uploadConfigurationOptions.Object, kafkaConfigOptions.Object, kafkaProducerService.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Message.ShouldContain("Kafka error");

        importRepository.Verify(x => x.CreateAsync(It.IsAny<Import>()), Times.Once);
        kafkaProducerService.Verify(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<Import>(), It.IsAny<string>()),
            Times.Once);
        s3Service.Verify(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>(), It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task Should_ReturnFailure_When_ImportRepositoryThrowsException()
    {
        // Arrange
        Mock<IS3Service> s3Service = new();
        Mock<IKafkaProducerService> kafkaProducerService = new();
        Mock<IImportRepository> importRepository = new();
        Mock<IOptions<FileUploadConfiguration>> uploadConfigurationOptions = new();
        FileUploadConfiguration uploadConfiguration = new() { BucketName = "test-bucket", DNS = "http://minio:0000" };
        Mock<IOptions<KafkaConfiguration>> kafkaConfigOptions = new();
        KafkaConfiguration kafkaConfiguration = new() { Endpoint = "kafka:00000", ImportsTopic = "some-kafka-topic" };

        s3Service.Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        importRepository.Setup(x => x.CreateAsync(It.IsAny<Import>()))
            .ThrowsAsync(new Exception("Database error"));

        kafkaProducerService.Setup(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<Import>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        uploadConfigurationOptions.Setup(x => x.Value).Returns(uploadConfiguration);

        kafkaConfigOptions.Setup(x => x.Value).Returns(kafkaConfiguration);

        var filebytes = Encoding.UTF8.GetBytes("sample-data");
        IFormFile file = new FormFile(new MemoryStream(filebytes), 0, filebytes.Length, "Data", "sample-data.csv")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/csv"
        };

        CreateImportRequest importRequest = new(file);

        CreateImportCommand command = new(importRequest);

        CreateImportCommandHandler handler = new(s3Service.Object, importRepository.Object,
            uploadConfigurationOptions.Object, kafkaConfigOptions.Object, kafkaProducerService.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Message.ShouldContain("Database error");

        importRepository.Verify(x => x.CreateAsync(It.IsAny<Import>()), Times.Once);
        kafkaProducerService.Verify(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<Import>(), It.IsAny<string>()),
            Times.Never);
        s3Service.Verify(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>(), It.IsAny<string>()),
            Times.Once);
    }
}