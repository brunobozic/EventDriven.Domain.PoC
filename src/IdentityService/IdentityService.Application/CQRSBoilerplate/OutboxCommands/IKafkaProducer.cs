using SharedKernel.DomainContracts;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityService.Application.CQRSBoilerplate.OutboxCommands;

internal interface IKafkaProducer
{
    Task Produce(IIntegrationEventNotification request, CancellationToken cancellationToken);
}