namespace EventDriven.Domain.PoC.Application.DomainServices.JournalServices
{
    public class CreateJournalEntryResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}