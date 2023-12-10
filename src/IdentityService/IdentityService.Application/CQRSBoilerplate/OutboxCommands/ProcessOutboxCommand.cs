using MediatR;
using SharedKernel.DomainContracts;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application.CQRSBoilerplate.OutboxCommands;

public class ProcessOutboxCommand : CommandBase<Unit>, IRecurringCommand
{
}