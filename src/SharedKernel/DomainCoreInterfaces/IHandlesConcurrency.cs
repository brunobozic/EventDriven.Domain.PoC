namespace SharedKernel.DomainCoreInterfaces;

public interface IHandlesConcurrency
{
    byte[] RowVersion { get; set; }
}