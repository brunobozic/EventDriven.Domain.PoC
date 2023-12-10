using System;
using System.Threading.Tasks;

namespace SharedKernel.DomainContracts;

public interface ICommandsDispatcher
{
    Task DispatchCommandAsync(Guid id);
}