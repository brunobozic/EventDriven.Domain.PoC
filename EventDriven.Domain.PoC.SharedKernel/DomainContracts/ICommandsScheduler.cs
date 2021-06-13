using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.SharedKernel.DomainContracts
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync<T>(ICommand<T> command);
    }
}