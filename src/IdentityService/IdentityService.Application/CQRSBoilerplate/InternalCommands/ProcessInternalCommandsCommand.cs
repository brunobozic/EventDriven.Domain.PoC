using MediatR;
using SharedKernel.DomainContracts;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application.CQRSBoilerplate.InternalCommands;

public class ProcessInternalCommandsCommand : CommandBase<Unit>, IRecurringCommand
{
}