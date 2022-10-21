using AutoMapper;
using EventDriven.Domain.PoC.Application.DomainServices.EmailServices;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AccountJournal;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork.Interfaces;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace EventDriven.Domain.PoC.Application.DomainServices.JournalServices
{
    public class JournalService : Service<AccountJournalEntry>, IJournalService
    {
        private readonly IMyUnitOfWork UnitOfWork;
        private readonly ITrackableRepository<Role> RoleRepository;
        private readonly ITrackableRepository<User> UserRepository;
        private readonly ITrackableRepository<AccountJournalEntry> JournalRepository;
        private readonly IMapper _mapper;
        private readonly MyConfigurationValues _appSettings;
        private readonly IEmailService _emailService;

        public JournalService(
           IMyUnitOfWork unitOfWork,
           ITrackableRepository<User> repository,
           ITrackableRepository<Role> roleRepository,
           ITrackableRepository<AccountJournalEntry> journalRepository,
           IMapper mapper,
           IOptions<JwtIssuerOptions> jwtOptions,
           IOptions<MyConfigurationValues> appSettings,
           IEmailService emailService) : base(journalRepository)
        {
            UnitOfWork = unitOfWork;
            UserRepository = repository;
            RoleRepository = roleRepository;
            JournalRepository = journalRepository;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }

        public async Task<CreateJournalEntryResponse> CreateAsync(string journalEntryMessage, Guid actor, Guid actedUpon)
        {
            var retVal = new CreateJournalEntryResponse();

            try
            {
                if (string.IsNullOrEmpty(journalEntryMessage)) { throw new ArgumentNullException(nameof(journalEntryMessage)); };
                if (Guid.Empty == actor) { throw new ArgumentNullException(nameof(actor)); };
                if (Guid.Empty == actedUpon) { throw new ArgumentNullException(nameof(actedUpon)); };

                var actorEntity = await UserRepository.Queryable().Where(user => user.Id == actor).SingleAsync();
                var actedUponEntity = await UserRepository.Queryable().Where(user => user.Id == actedUpon).SingleAsync();

                var entry = new AccountJournalEntry(journalEntryMessage);
                entry.AttachActingUser(actorEntity);
                entry.AttachUser(actedUponEntity);

                var saved = await UnitOfWork.SaveChangesAsync();

                retVal.Success = true;
                retVal.Message = "Journal entry persisted successfully.";
            }
            catch (Exception ex)
            {
                retVal.Message = ex.Message;
            }

            return retVal;
        }

        public async Task<GetAllJournalEntriesResponse> GetAllAsync()
        {
            var retVal = new GetAllJournalEntriesResponse();

            try
            {
                var listOfEntries = await JournalRepository.Queryable().ToListAsync();

                var mapped = _mapper.Map<List<AccountJournalEntryViewModel>>(listOfEntries);

                retVal.ViewModel = mapped;
                retVal.Success = true;
                retVal.Message = $"Returned [ {listOfEntries.Count} ] entries.";
            }
            catch (Exception ex)
            {
                retVal.Message = ex.Message;
            }

            return retVal;
        }

        public async Task<GetJournalEntriesResponse> GetByUserIdAsync(Guid userId)
        {
            var retVal = new GetJournalEntriesResponse();

            try
            {
                if (Guid.Empty == userId) { throw new ArgumentNullException(nameof(userId)); };
                var entry = await JournalRepository.Queryable().Where(e => e.UserActedUponId == userId).ToListAsync();

                //var mapped = _mapper.Map<AccountJournalEntryViewModel>(entry);

                retVal.ViewModel.ListOfEntries = entry;
                retVal.Success = true;
                retVal.Message = $"Returned [ {1} ] entries.";
            }
            catch (Exception ex)
            {
                retVal.Message = ex.Message;
            }

            return retVal;
        }
    }
}