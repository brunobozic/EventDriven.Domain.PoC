namespace EventDriven.Domain.PoC.Application.DomainServices.JournalServices
{
    public class GetJournalEntriesResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public AccountJournalEntryViewModel ViewModel { get; set; }
    }
}