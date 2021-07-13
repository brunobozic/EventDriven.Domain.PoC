﻿using System;

namespace EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces
{
    public interface ICreationAuditedEntity
    {
        DateTimeOffset DateCreated { get; set; }

        Guid? CreatedById { get; set; }
        bool IsDraft { get; set; }
        bool IsSeed { get; set; }
    }
}