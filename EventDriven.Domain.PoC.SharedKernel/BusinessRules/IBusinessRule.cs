namespace EventDriven.Domain.PoC.SharedKernel.BusinessRules
{
    public interface IBusinessRule
    {
        string Message { get; }
        bool IsBroken();
    }
}