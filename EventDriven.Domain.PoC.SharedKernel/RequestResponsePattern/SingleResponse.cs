namespace EventDriven.Domain.PoC.SharedKernel.RequestResponsePattern
{
    public class SingleResponse<TModel> : ISingleResponse<TModel>
    {
        public string Message { get; set; }
        public bool IsSuccessful { get; set; }
        public TModel ViewModel { get; set; }
    }
}