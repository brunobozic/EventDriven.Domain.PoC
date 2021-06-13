using MediatR;

namespace EventDriven.Domain.PoC.SharedKernel.DomainContracts
{
    public interface IQueryHandler<in TQuery, TResult> :
        IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
    }
}