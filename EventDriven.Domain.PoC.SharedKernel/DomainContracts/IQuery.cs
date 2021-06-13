using MediatR;

namespace EventDriven.Domain.PoC.SharedKernel.DomainContracts
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}