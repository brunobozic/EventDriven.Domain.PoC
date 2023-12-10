using System.Threading.Tasks;

namespace SharedKernel.DomainContracts;

public interface ICommandsScheduler
{
    Task EnqueueAsync<T>(ICommand<T> command);
}