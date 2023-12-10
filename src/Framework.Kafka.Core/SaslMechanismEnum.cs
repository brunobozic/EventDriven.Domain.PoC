using System.ComponentModel;

namespace Framework.Kafka.Core;

public enum SaslMechanismEnum
{
    [Description("PLAIN")] Plain,
    GSSAPI
}