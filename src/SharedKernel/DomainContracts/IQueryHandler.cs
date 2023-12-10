using MediatR;

namespace SharedKernel.DomainContracts;

public interface IQueryHandler<in TQuery, TResult> :
    IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
}