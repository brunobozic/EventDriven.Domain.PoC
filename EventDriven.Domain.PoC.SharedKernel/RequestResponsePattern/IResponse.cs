namespace EventDriven.Domain.PoC.SharedKernel.RequestResponsePattern
{
    public interface IResponse
    {
        string Message { get; set; }

        bool IsSuccessful { get; set; }
    }

    public interface IResponse<TModel>
    {
    }
}