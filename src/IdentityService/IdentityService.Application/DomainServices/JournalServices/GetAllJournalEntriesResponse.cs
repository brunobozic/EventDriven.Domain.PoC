using System.Collections.Generic;

namespace IdentityService.Application.DomainServices.JournalServices;

public class GetAllJournalEntriesResponse
{
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; } = false;
    internal List<AccountJournalEntryViewModel> ViewModel { get; set; } = new();
}