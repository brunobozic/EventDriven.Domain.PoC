using SharedKernel.DomainContracts;
using System.Threading.Tasks;


public interface IIntegrationEventPersistor
{
    Task InsertIntoInbox(IIntegrationEvent<IDomainEvent> @event);
}

