using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces;
using TrackableEntities.Common.Core;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.Audit
{
    // http://www.codepro.co.nz/blog/2018/01/add-audit-table-to-net-core-2-app/
    [Table("DbAuditTrail", Schema = "Audit")]
    public class AuditTrail : IAuditTrail
    {
        public long AuditTrailId { get; set; }
        public string TableName { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public long? TableIdValue { get; set; }
        public Guid TableIdValueGuid { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string Actions { get; set; }

        #region ITrackable

        [NotMapped] public TrackingState TrackingState { get; set; }

        [NotMapped] public ICollection<string> ModifiedProperties { get; set; }

        #endregion ITrackable
    }
}