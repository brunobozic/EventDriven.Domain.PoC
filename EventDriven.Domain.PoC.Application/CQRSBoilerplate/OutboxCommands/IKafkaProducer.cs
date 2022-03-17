using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using System.Threading;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.Application.CQRSBoilerplate.OutboxCommands
{
    internal interface IKafkaProducer
    {
        Task Produce(IIntegrationEventNotification request, CancellationToken cancellationToken);
    }
}