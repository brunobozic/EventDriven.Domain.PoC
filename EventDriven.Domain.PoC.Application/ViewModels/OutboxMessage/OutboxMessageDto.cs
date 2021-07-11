namespace EventDriven.Domain.PoC.Application.ViewModels.OutboxMessage
{
    public class OutboxMessageDto
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }
    }
}