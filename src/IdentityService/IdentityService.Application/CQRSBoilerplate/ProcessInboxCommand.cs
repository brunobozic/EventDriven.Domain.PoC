using SharedKernel.DomainContracts;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application.CQRSBoilerplate;

public class ProcessInboxCommand : CommandBase, IRecurringCommand
{
}
