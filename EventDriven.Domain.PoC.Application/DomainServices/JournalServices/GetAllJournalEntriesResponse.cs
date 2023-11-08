using System.Collections.Generic;

namespace EventDriven.Domain.PoC.Application.DomainServices.JournalServices
{
    public class GetAllJournalEntriesResponse
    {
        public GetAllJournalEntriesResponse()
        {
        }

        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; } = false;
        internal List<AccountJournalEntryViewModel> ViewModel { get; set; } = new List<AccountJournalEntryViewModel>();
    }
}