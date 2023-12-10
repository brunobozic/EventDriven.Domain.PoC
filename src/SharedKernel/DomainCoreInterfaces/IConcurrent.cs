using System.ComponentModel.DataAnnotations;

namespace SharedKernel.DomainCoreInterfaces;

public interface IConcurrent
{
    [Timestamp] byte[] RowVersion { get; set; }
}