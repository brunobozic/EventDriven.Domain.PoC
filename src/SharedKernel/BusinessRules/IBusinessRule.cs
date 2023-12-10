namespace SharedKernel.BusinessRules;

public interface IBusinessRule
{
    string Message { get; }

    bool IsBroken();
}