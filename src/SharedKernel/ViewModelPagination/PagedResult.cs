using System.Collections.Generic;

namespace SharedKernel.ViewModelPagination;

public class PagedResult<T>
{
    public int Count { get; set; }

    public IEnumerable<T> Data { get; set; }
    public int PageCount { get; set; }
}