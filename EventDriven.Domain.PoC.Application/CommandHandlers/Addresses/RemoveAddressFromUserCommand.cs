using System;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.Addresses
{
    public class RemoveAddressFromUserCommand : CommandBase<AddressAssignmentDto>
    {
        public RemoveAddressFromUserCommand(Guid userId, Guid roleId, string addressName)
        {
            this.UserId = userId;
            this.RoleId = roleId;
            this.AddressName = addressName;
        }

        private Guid RoleId { get; set; }

        private string AddressName { get; set; }


        private Guid UserId { get; set; }
        public Guid RemoverUser { get; set; }

        public string Origin { get; set; }
    }
}