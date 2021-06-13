using System;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.SharedKernel.DomainContracts
{
    public interface ICommandsDispatcher
    {
        Task DispatchCommandAsync(Guid id);
    }
}