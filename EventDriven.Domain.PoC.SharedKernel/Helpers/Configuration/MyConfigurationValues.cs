using EventDriven.Domain.PoC.SharedKernel.Helpers.EmailSender;
using EventDriven.Domain.PoC.SharedKernel.Kafka.Settings;
using Framework.Kafka.Core.KafkaSettings;
using System.Net;

namespace EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration
{
    public class ConsulOptions
    {
        public DnsEndpoint DnsEndpoint { get; set; }
        public string HttpEndpoint { get; set; }
    }

    public class DnsEndpoint
    {
        public string Address { get; set; }

        public int Port { get; set; }

        public IPEndPoint ToIPEndPoint()
        {
            return new(IPAddress.Parse(Address), Port);
        }
    }

    public class MinimumLevel
    {
        public string Default { get; set; }
        public Override Override { get; set; }
    }

    public class MyConfigurationValues : IMyConfigurationValues
    {
        public string ActiveDirectoryIntegrationTurnedOff { get; set; } =
            "Active Directory integration is currently turned off.";

        public string CacheTimeoutInSeconds { get; set; }
        public string CachingIsEnabled { get; set; }
        public bool CorrelationIdEmission { get; set; }
        public DeadLetterArchiveJobSettings DeadLetterArchiveJobSettings { get; set; }
        public DeadLetterOutboxJobSettings DeadLetterOutboxJobSettings { get; set; }
        public int DefaultPageNumber { get; set; } = 1;
        public int DefaultPageSize { get; set; } = 20;
        public string ElasticsearchUrl { get; set; }
        public string Environment { get; set; }
        public string GenericErrorMessageForEndUser { get; set; }
        public string InstanceName { get; set; }
        public KafkaConsumerSettings KafkaConsumerSettings { get; set; }
        public KafkaLoggingProducerSettings KafkaLoggingProducerSettings { get; set; }
        public KafkaProducerSettings KafkaProducerSettings { get; set; }
        public int? MainKafkaMessagePollInterval { get; set; }
        public string NotFoundErrorMessageForEndUser { get; set; }
        public PollySettings PollySettings { get; set; }
        public long RefreshTokenTTL { get; set; }
        public bool ResponseCaching { get; set; }
        public string Secret { get; set; }
        public SerilogOptions Serilog { get; set; }
        public bool SerilogConsole { get; set; }
        public bool SerilogElasticSearch { get; set; }
        public bool SerilogLofToFile { get; set; }
        public SmtpOptions SmtpOptions { get; set; }
        public bool UseActiveDirectory { get; set; } = false;
        public string WorkerName { get; set; }
    }

    public class Override
    {
        public string Microsoft { get; set; }
        public string MicrosoftAspNetCore { get; set; }
        public string System { get; set; }
    }

    public class SerilogOptions
    {
        public string ConnectionString { get; set; }
        public MinimumLevel MinimumLevel { get; set; }
        public string TableName { get; set; }
    }

    public class ServiceDisvoveryOptions
    {
        public ConsulOptions Consul { get; set; }
        public string ServiceName { get; set; }
    }
}