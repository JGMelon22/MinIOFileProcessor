using FileUploaderPartA.Infrastructure.Interfaces.Repository;
using FileUploaderPartA.Infrastructure.Interfaces.Services;
using FileUploaderPartA.Infrastructure.Repositories;
using FileUploaderPartA.Infrastructure.Services;

namespace FileUploaderPartA.API.Extensions;

public static class IocExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IS3Service, S3Service>();
        services.AddTransient<S3Service>();
        services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
        
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IImportRepository, ImportRepository>();

        return services;
    }
}