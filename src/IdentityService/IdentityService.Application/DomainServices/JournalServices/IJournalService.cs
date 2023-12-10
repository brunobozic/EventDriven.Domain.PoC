using System;
using System.Threading.Tasks;
using IdentityService.Domain.DomainEntities.UserAggregate.AccountJournal;
using URF.Core.Abstractions.Services;

namespace IdentityService.Application.DomainServices.JournalServices;

public interface IJournalService : IService<AccountJournalEntry>
{
    Task<CreateJournalEntryResponse> CreateAsync(string journalEntryMessage, Guid actor, Guid actedUpon);

    Task<GetAllJournalEntriesResponse> GetAllAsync();

    Task<GetJournalEntriesResponse> GetByUserIdAsync(Guid userId);
}