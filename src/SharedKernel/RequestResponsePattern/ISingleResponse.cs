namespace SharedKernel.RequestResponsePattern;

public interface ISingleResponse<TModel> : IResponse
{
    TModel ViewModel { get; set; }
}