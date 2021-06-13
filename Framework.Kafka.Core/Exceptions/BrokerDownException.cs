using System;

namespace Framework.Kafka.Core.Exceptions
{
    public class BrokerDownException : Exception
    {
        public BrokerDownException(string message) : base(message)
        {
        }
    }
}