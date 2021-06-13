using System.Collections.Generic;

namespace EventDriven.Domain.PoC.SharedKernel.RequestResponsePattern
{
    public class ListResponse<TModel> : IListResponse<TModel>
    {
        public string ErrorMessage { get; set; }
        public string Message { get; set; }

        public bool IsSuccessful { get; set; }

        public IEnumerable<TModel> Model { get; set; }
    }
}