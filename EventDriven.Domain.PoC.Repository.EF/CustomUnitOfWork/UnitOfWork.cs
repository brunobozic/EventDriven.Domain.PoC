using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork.Interfaces;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using URF.Core.EF;

namespace EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork
{
    public class MyUnitOfWork : UnitOfWork, IMyUnitOfWork
    {
        public MyUnitOfWork(
            DbContext context,
            IDomainEventsDispatcher domainEventsDispatcher) : base(context)
        {
            _context = context;
            DomainEventsDispatcher = domainEventsDispatcher;
        }

        // ReSharper disable once InconsistentNaming
        private DbContext _context { get; }

        public IDomainEventsDispatcher DomainEventsDispatcher { get; }

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        {
            // this will throw if, for an example, it cant resolve the event type
            // It would not be wise to proceed with saving the changes to the Db in this case, as
            // that might lead to inconsistencies (domain event not being fired, but an entity created or mutated)
            await DomainEventsDispatcher.DispatchEventsAsync();
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}