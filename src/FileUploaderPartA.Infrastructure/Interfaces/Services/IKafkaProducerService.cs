namespace FileUploaderPartA.Infrastructure.Interfaces.Services;

public interface IKafkaProducerService
{
    Task<bool> ProduceAsync<T>(string key, T message, string topic);
}