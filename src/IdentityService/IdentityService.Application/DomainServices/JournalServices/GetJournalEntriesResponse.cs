namespace IdentityService.Application.DomainServices.JournalServices;

public class GetJournalEntriesResponse
{
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; } = false;
    public AccountJournalEntryViewModel ViewModel { get; set; }
}