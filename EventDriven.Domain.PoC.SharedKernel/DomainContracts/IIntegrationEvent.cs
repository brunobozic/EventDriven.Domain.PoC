﻿using System;
using MediatR;

namespace EventDriven.Domain.PoC.SharedKernel.DomainContracts
{
    public interface IIntegrationEvent<out TEventType> : IIntegrationEventNotification
    {
        TEventType IntegrationEvent { get; }
    }

    public interface IIntegrationEventNotification : INotification
    {
        Guid Id { get; }
    }
}