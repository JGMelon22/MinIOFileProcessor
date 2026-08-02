using FileUploaderPartA.Application.Imports.Commands;
using FileUploaderPartA.Application.Imports.Commands.Handlers;
using FileUploaderPartA.Core.Shared;
using FileUploaderPartA.Infrastructure.Interfaces.Repository;
using FileUploaderPartA.Infrastructure.Interfaces.Services;
using FileUploaderPartA.Infrastructure.Repositories;
using FileUploaderPartA.Infrastructure.Services;
using NetDevPack.SimpleMediator;

namespace FileUploaderPartA.API.Extensions;

public static class IocExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IS3Service, S3Service>();
        services.AddSingleton<IKafkaProducerService, KafkaProducerService>();

        services.AddHealthChecksUI()
            .AddInMemoryStorage();

        services.AddHealthChecks();
        return services;
    }

    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();

        // Alunos - Commands
        services.AddScoped<IRequestHandler<CreateImportCommand, Result<bool>>, CreateImportCommandHandler>();

        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IImportRepository, ImportRepository>();

        return services;
    }
}