using Confluent.Kafka;
using FileUploaderPartA.Infrastructure.Configurations;
using FileUploaderPartA.Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FileUploaderPartA.Infrastructure.Services;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaProducerService> _logger;

    public KafkaProducerService(IOptions<KafkaConfiguration> config, ILogger<KafkaProducerService> logger)
    {
        _producer = new ProducerBuilder<string, string>(new ProducerConfig
        {
            BootstrapServers = config.Value.Endpoint
        })
        .Build();
        _logger = logger;
    }

    public async Task ProduceAsync<T>(string key, T message, string topic)
    {
        JsonSerializerOptions jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };

        string jsonMessage = JsonSerializer.Serialize(message, jsonOptions);

        try
        {
            _logger.LogInformation(
                "{ServiceName}.{MethodName} - Start: Producing message with key '{Key}' to topic '{Topic}'.",
                nameof(KafkaProducerService),
                nameof(ProduceAsync),
                key,
                topic);

            await _producer.ProduceAsync(topic, new Message<string, string>
            {
                Key = key,
                Value = jsonMessage
            });

        }
        catch (ProduceException<string, string> ex)
        {
            _logger.LogError(
                ex,
                "{ServiceName}.{MethodName} - Error: Failed to produce message with key '{Key}' to topic '{Topic}'.",
                nameof(KafkaProducerService),
                nameof(ProduceAsync),
                key,
                topic);

            throw;
        }
    }
}