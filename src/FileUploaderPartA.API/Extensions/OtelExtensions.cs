using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace FileUploaderPartA.API.Extensions;

public static class OtelExtensions
{
    public static void AddOpenTelemetryServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
            .WithMetrics(options =>
            {
                options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("FileUploader.API"))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddOtlpExporter(otel =>
                    {
                        otel.Protocol = OtlpExportProtocol.Grpc;
                        otel.Endpoint = new Uri(builder.Configuration["OtlpExporter:Endpoint"]!);
                    });
            })
            .WithTracing(options =>
            {
                options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("FileUploader.API"))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(otel =>
                    {
                        otel.Protocol = OtlpExportProtocol.Grpc;
                        otel.Endpoint = new Uri(builder.Configuration["OtlpExporter:Endpoint"]!);
                    });
            });
    }
}