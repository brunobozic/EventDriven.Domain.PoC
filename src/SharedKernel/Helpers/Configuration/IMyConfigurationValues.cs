using SharedKernel.Kafka.Settings;

namespace SharedKernel.Helpers.Configuration;

public interface IMyConfigurationValues
{
    PollySettings PollySettings { get; set; }
}