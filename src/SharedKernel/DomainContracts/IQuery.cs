using MediatR;

namespace SharedKernel.DomainContracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}