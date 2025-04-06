namespace FileUploaderPartA.Infrastructure.Configurations;

public class KafkaConfiguration
{
    public string Endpoint { get; set; } = string.Empty;
    public string ImportsTopic { get; set; } = string.Empty;
}