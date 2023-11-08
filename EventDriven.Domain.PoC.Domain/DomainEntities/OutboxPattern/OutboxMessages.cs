using System;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.OutboxPattern
{
    public class OutboxMessage
    {
        public OutboxMessage(DateTime occurredOn, string type, string data)
        {
            Id = Guid.NewGuid();
            OccurredOn = occurredOn;
            Type = type;
            Data = data;
        }

        private OutboxMessage()
        {
        }

        public string Data { get; set; }
        public Guid Id { get; set; }

        public DateTime OccurredOn { get; set; }

        public DateTime? ProcessedDate { get; set; }
        public string Type { get; set; }
    }
}