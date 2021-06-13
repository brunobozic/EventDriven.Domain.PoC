using System.ComponentModel;

namespace Framework.Kafka.Core
{
    public enum SecurityProtocolEnum
    {
        [Description("PLAINTEXT")] PlainText,

        [Description("SASL_PLAINTEXT")] SASL_Plaintext
    }
}