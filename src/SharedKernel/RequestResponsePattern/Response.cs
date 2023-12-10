namespace SharedKernel.RequestResponsePattern;

public class Response<TModel> : IResponse<TModel>
{
    public bool DidError { get; set; }
    public string ErrorMessage { get; set; }
    public string Message { get; set; }
    public TModel Model { get; set; }
    public bool ModelCreated { get; set; }
    public bool ModelDeleted { get; set; }
}