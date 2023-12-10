namespace SharedKernel.RequestResponsePattern;

public class SingleResponse<TModel> : ISingleResponse<TModel>
{
    public bool IsSuccessful { get; set; }
    public string Message { get; set; }
    public TModel ViewModel { get; set; }
}