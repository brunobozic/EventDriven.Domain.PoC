namespace SharedKernel.RequestResponsePattern;

public interface IResponse
{
    bool IsSuccessful { get; set; }
    string Message { get; set; }
}

public interface IResponse<TModel>
{
}