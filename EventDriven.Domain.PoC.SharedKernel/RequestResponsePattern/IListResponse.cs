using System.Collections.Generic;

namespace EventDriven.Domain.PoC.SharedKernel.RequestResponsePattern
{
    public interface IListResponse<TModel> : IResponse
    {
        IEnumerable<TModel> Model { get; set; }
    }
}