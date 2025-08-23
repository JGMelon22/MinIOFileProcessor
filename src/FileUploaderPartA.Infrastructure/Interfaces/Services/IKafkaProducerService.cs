namespace FileUploaderPartA.Infrastructure.Interfaces.Services;

public interface IKafkaProducerService
{
    Task ProduceAsync<T>(string key, T message, string topic);
}