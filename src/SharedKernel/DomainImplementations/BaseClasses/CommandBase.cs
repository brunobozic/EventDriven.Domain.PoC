﻿using System;
using SharedKernel.DomainContracts;

namespace SharedKernel.DomainImplementations.BaseClasses;

public class CommandBase : ICommand
{
    public CommandBase()
    {
        Id = Guid.NewGuid();
    }

    protected CommandBase(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}

public abstract class CommandBase<TResult> : ICommand<TResult>
{
    protected CommandBase()
    {
        Id = Guid.NewGuid();
    }

    protected CommandBase(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}