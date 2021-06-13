using System.ComponentModel;

namespace EventDriven.Domain.PoC.SharedKernel.Kafka
{
    public enum SecurityProtocolEnum
    {
        [Description("PLAINTEXT")] PlainText,

        [Description("SASL_PLAINTEXT")] SASL_Plaintext
    }
}