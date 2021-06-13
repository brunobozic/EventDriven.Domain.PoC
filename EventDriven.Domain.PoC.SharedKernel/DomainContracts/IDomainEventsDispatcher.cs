using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.SharedKernel.DomainContracts
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}