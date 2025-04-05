using FileUploaderPartA.Core.Domains.Imports.Dtos;
using FileUploaderPartA.Core.Shared;
using MediatR;

namespace FileUploaderPartA.Application.Imports.Commands;

public record CreateImportCommand(CreateImportRequest request) : IRequest<Result<bool>>;
