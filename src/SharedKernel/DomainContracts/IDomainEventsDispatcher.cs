using System.Threading.Tasks;

namespace SharedKernel.DomainContracts;

public interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync();
}