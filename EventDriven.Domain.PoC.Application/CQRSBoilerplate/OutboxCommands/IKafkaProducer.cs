using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;

namespace EventDriven.Domain.PoC.Application.CQRSBoilerplate.OutboxCommands
{
    internal interface IKafkaProducer
    {
        Task Produce(IIntegrationEventNotification request, CancellationToken cancellationToken);
    }
}