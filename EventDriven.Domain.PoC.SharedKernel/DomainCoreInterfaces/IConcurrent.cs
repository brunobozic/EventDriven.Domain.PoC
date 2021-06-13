using System.ComponentModel.DataAnnotations;

namespace EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces
{
    public interface IConcurrent
    {
        [Timestamp] byte[] RowVersion { get; set; }
    }
}