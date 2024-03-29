﻿using System;
using MediatR;

namespace EventDriven.Domain.PoC.SharedKernel.DomainContracts
{
    public interface ICommand : IRequest
    {
        Guid Id { get; }
    }

    public interface ICommand<out TResult> : IRequest<TResult>
    {
        Guid Id { get; }
    }
}