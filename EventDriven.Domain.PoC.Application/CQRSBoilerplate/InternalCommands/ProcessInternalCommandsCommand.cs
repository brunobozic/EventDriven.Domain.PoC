using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using MediatR;

namespace EventDriven.Domain.PoC.Application.CQRSBoilerplate.InternalCommands
{
    public class ProcessInternalCommandsCommand : CommandBase<Unit>, IRecurringCommand
    {
    }
}