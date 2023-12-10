using System;

namespace IdentityService.Domain.DomainEntities.OutboxPattern;

public class InternalCommand
{
    public string Data { get; set; }
    public DateTime? EnqueueDate { get; set; }
    public Guid Id { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public string Type { get; set; }
}