using System.Collections.Generic;

namespace SharedKernel.RequestResponsePattern;

public interface IListResponse<TModel> : IResponse
{
    IEnumerable<TModel> Model { get; set; }
}