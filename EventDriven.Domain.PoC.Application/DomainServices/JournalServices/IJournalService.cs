using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AccountJournal;
using System;
using System.Threading.Tasks;
using URF.Core.Abstractions.Services;

namespace EventDriven.Domain.PoC.Application.DomainServices.JournalServices
{
    public interface IJournalService : IService<AccountJournalEntry>
    {
        Task<CreateJournalEntryResponse> CreateAsync(string journalEntryMessage, Guid actor, Guid actedUpon);

        Task<GetAllJournalEntriesResponse> GetAllAsync();

        Task<GetJournalEntriesResponse> GetByUserIdAsync(Guid userId);
    }
}