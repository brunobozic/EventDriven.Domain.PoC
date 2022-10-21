﻿using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using System;

namespace EventDriven.Domain.PoC.Application.CommandsAndHandlers.Users.Email
{
    public class MarkUserAsWelcomedCommand : ICommand<object>
    {
        private Guid guid;
        private Guid userId;

        public MarkUserAsWelcomedCommand(Guid guid, Guid userId)
        {
            this.guid = guid;
            this.userId = userId;
        }

        public Guid Id => Guid.NewGuid();
    }
}