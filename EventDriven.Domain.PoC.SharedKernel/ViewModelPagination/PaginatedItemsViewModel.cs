using System.Collections.Generic;

namespace EventDriven.Domain.PoC.SharedKernel.ViewModelPagination
{
    public class PaginatedItemsViewModel<TEntity> where TEntity : class
    {
        public PaginatedItemsViewModel(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        public long Count { get; }
        public IEnumerable<TEntity> Data { get; }
        public int PageIndex { get; }

        public int PageSize { get; }
    }
}