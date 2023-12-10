using System.ComponentModel;

namespace SharedKernel.Kafka;

public enum SecurityProtocolEnum
{
    [Description("PLAINTEXT")] PlainText,

    [Description("SASL_PLAINTEXT")] SASL_Plaintext,
    Kerberos
}