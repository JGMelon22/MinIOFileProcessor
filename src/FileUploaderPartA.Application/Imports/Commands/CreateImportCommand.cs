using FileUploaderPartA.Core.Domains.Imports.Dtos;
using FileUploaderPartA.Core.Shared;
using NetDevPack.SimpleMediator;

namespace FileUploaderPartA.Application.Imports.Commands;

public record CreateImportCommand(CreateImportRequest Request) : IRequest<Result<bool>>;