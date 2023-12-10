using System.Threading;
using System.Threading.Tasks;
using SharedKernel.DomainContracts;

namespace IdentityService.Application.CQRSBoilerplate.OutboxCommands;

internal interface IKafkaProducer
{
    Task Produce(IIntegrationEventNotification request, CancellationToken cancellationToken);
}