using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate.AddressDomainEvents;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Roles.Handlers
{
    public class AddressRemovedFromUserNotification : IntegrationEventBase<AddressRemovedFromUserDomainEvent>
    {
        public bool AddressActive { get; set; }
        public string AddressTypeDescription { get; set; }
        public long AddressTypeID { get; set; }
        public string AddressTypeName { get; set; }
        public Guid Assignee { get; set; }
        public string AssigneeEmail { get; set; }
        public string AssigneeUsername { get; set; }
        public Guid AssignerId { get; set; }
        public string AssignerUserName { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateModified { get; set; }
        public DateTimeOffset TownDateCreated { get; set; }
        public DateTimeOffset? TownDateModified { get; set; }
        public long TownId { get; set; }
        public string TownName { get; set; }
        public string TownZipCode { get; set; }
        public DateTimeOffset AddressDateCreated { get; set; }
        public DateTimeOffset? AddressDateModified { get; set; }
        public int? AddressFlatNr { get; set; }
        public int AddressHouseNumber { get; set; }
        public string AddressHouseNumberSuffix { get; set; }
        public string AddressPostalCode { get; set; }
        public bool AddressTypeActive { get; set; }
        public long AddressId { get; set; }
        public Guid AddressIdGuid { get; set; }
        public string AddressLine1 { get; set; }
        public AddressRemovedFromUserNotification(AddressRemovedFromUserDomainEvent integrationEvent) : base(integrationEvent)
        {
            AddressActive = integrationEvent.AddressActive;
            AddressTypeDescription = integrationEvent.AddressTypeDescription;
            AddressDateCreated = integrationEvent.AddressDateCreated;
            AddressDateModified = integrationEvent.AddressDateModified;
            AddressFlatNr = integrationEvent.AddressFlatNr;
            AddressHouseNumber = integrationEvent.AddressHouseNumber;
            AddressHouseNumberSuffix = integrationEvent.AddressHouseNumberSuffix;

            AddressId = integrationEvent.AddressId;
            AddressIdGuid = integrationEvent.AddressIdGuid;
            AddressLine1 = integrationEvent.AddressLine1;
            AddressHouseNumberSuffix = integrationEvent.AddressHouseNumberSuffix;
            AddressPostalCode = integrationEvent.AddressPostalCode;
            AddressTypeActive = integrationEvent.AddressTypeActive;
            AddressTypeDescription = integrationEvent.AddressTypeDescription;
            AddressTypeID = integrationEvent.AddressTypeID;
            AddressTypeName = integrationEvent.AddressTypeName;
            Assignee = integrationEvent.Assignee;
            AssigneeEmail = integrationEvent.AssigneeEmail;
            AssigneeUsername = integrationEvent.AssigneeUsername;
            AssignerId = integrationEvent.AssignerId;
            AssignerUserName = integrationEvent.AssignerUserName;
            DateCreated = integrationEvent.DateCreated;
            DateModified = integrationEvent.DateModified;

            TownDateCreated = integrationEvent.TownDateCreated;
            TownDateModified = integrationEvent.TownDateModified;
            TownId = integrationEvent.TownId;
            TownName = integrationEvent.TownName;
            TownZipCode = integrationEvent.TownZipCode;
        }
    }
}