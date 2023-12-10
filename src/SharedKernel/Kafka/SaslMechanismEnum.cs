using System.ComponentModel;

namespace SharedKernel.Kafka;

public enum SaslMechanismEnum
{
    [Description("PLAIN")] Plain,
    GSSAPI
}