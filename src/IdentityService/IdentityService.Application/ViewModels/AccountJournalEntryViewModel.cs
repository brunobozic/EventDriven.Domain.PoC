using IdentityService.Domain.DomainEntities.UserAggregate.AccountJournal;
using System.Collections.Generic;

namespace IdentityService.Application.ViewModels;

public class AccountJournalEntryViewModel
{
    public List<AccountJournalEntry> ListOfEntries { get; internal set; }
}