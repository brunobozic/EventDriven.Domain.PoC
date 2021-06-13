namespace EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces
{
    public interface IHandlesConcurrency
    {
        byte[] RowVersion { get; set; }
    }
}