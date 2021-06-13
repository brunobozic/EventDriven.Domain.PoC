using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using MediatR;

namespace EventDriven.Domain.PoC.Application.CQRSBoilerplate.OutboxCommands
{
    public class ProcessOutboxCommand : CommandBase<Unit>, IRecurringCommand
    {
    }
}