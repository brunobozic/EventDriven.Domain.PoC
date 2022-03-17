using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AccountJournal;
using System.Collections.Generic;

namespace EventDriven.Domain.PoC.Application.DomainServices.JournalServices
{
    public class AccountJournalEntryViewModel
    {
        public List<AccountJournalEntry> ListOfEntries { get; internal set; }
    }
}