using FileUploaderPartA.Infrastructure.Interfaces.Services;
using FileUploaderPartA.Infrastructure.Services;

namespace FileUploaderPartA.API.Extensions;

public static class IocExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IS3Service, S3Service>();
        services.AddTransient<S3Service>();

        return services;
    }
}