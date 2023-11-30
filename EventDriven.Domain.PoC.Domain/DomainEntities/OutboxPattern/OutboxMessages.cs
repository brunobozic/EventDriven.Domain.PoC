using EventDriven.Domain.PoC.SharedKernel;
using System;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.OutboxPattern
{
    public class OutboxMessage
    {
        public OutboxMessage(DateTime occurredOn, string type, string data, EventTypeEnum eventType)
        {
            Id = Guid.NewGuid();
            OccurredOn = occurredOn;
            Type = type;
            Data = data;
            EventType = eventType;
        }

        private OutboxMessage()
        {
        }

        public string Data { get; set; }
        public Guid Id { get; set; }

        public DateTime OccurredOn { get; set; }

        public DateTime? ProcessedDate { get; set; }
        public string Type { get; set; }
        public EventTypeEnum EventType { get; set; }
    }
}