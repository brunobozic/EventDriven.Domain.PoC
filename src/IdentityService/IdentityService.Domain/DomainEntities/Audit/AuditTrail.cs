using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SharedKernel.DomainCoreInterfaces;
using TrackableEntities.Common.Core;

namespace IdentityService.Domain.DomainEntities.Audit;

// http://www.codepro.co.nz/blog/2018/01/add-audit-table-to-net-core-2-app/
[Table("DbAuditTrail", Schema = "Audit")]
public class AuditTrail : IAuditTrail
{
    public string Actions { get; set; }
    public long AuditTrailId { get; set; }
    public string NewData { get; set; }
    public string OldData { get; set; }
    public long? TableIdValue { get; set; }
    public Guid TableIdValueGuid { get; set; }
    public string TableName { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }

    #region ITrackable

    [NotMapped] public ICollection<string> ModifiedProperties { get; set; }
    [NotMapped] public TrackingState TrackingState { get; set; }

    #endregion ITrackable
}