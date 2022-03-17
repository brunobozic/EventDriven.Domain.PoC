using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace EventDriven.Domain.PoC.Application.CommandsAndHandlers.Addresses
{
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
}