﻿using System;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application.CommandsAndHandlers.Addresses;

public class RemoveAddressFromUserCommand : CommandBase<AddressAssignmentDto>
{
    public RemoveAddressFromUserCommand(Guid userId, Guid roleId, string addressName)
    {
        UserId = userId;
        RoleId = roleId;
        AddressName = addressName;
    }

    private Guid RoleId { get; }

    private string AddressName { get; }

    private Guid UserId { get; }
    public Guid RemoverUser { get; set; }

    public string Origin { get; set; }
}