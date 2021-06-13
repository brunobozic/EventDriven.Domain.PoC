namespace EventDriven.Domain.PoC.SharedKernel.RequestResponsePattern
{
    public class Response<TModel> : IResponse<TModel>
    {
        public bool ModelDeleted { get; set; }
        public bool ModelCreated { get; set; }
        public string Message { get; set; }

        public bool DidError { get; set; }

        public string ErrorMessage { get; set; }
        public TModel Model { get; set; }
    }
}