using System.Collections.Generic;

namespace EventDriven.Domain.PoC.SharedKernel.ViewModelPagination
{
    public class PagedResult<T>
    {
        public int Count { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<T> Data { get; set; }
    }
}