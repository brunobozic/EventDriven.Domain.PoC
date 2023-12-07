using EventDriven.Domain.PoC.SharedKernel.Kafka.Settings;

namespace EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration
{
    public interface IMyConfigurationValues
    {
        PollySettings PollySettings { get; set; }
    }
}