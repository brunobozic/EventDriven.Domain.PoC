using System.Collections.Generic;
using IdentityService.Domain.DomainEntities.UserAggregate.AccountJournal;

namespace IdentityService.Application.DomainServices.JournalServices;

public class AccountJournalEntryViewModel
{
    public List<AccountJournalEntry> ListOfEntries { get; internal set; }
}