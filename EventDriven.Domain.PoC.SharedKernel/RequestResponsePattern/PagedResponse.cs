using System.Collections.Generic;

namespace EventDriven.Domain.PoC.SharedKernel.RequestResponsePattern
{
    public class PagedResponse<TModel> : IPagedResponse<TModel>
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public string ErrorMessage { get; set; }

        public string OrderBy { get; set; }
        public string Message { get; set; }

        public bool IsSuccessful { get; set; }

        public IEnumerable<TModel> Model { get; set; }

        public int ItemsCount { get; set; }

        public double PageCount
            => ItemsCount < PageSize ? 1 : (int) ((double) ItemsCount / PageSize + 1);
    }
}