using EventDriven.Domain.PoC.Domain.DomainEntities.DomainExceptions;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork.Interfaces;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using URF.Core.Abstractions.Trackable;

namespace EventDriven.Domain.PoC.Application.CommandsAndHandlers.Addresses
{
    public class AssignAddressToUserCommandHandler : ICommandHandler<AssignAddressToUserCommand, AddressAssignmentDto>
    {
        #region ctor

        public AssignAddressToUserCommandHandler(
            IMyUnitOfWork unitOfWork,
            ITrackableRepository<User> userRepository,
            ITrackableRepository<Address> addressRepository,
            ITrackableRepository<AddressType> addressTypeRepository
        )
        {
            UnitOfWork = unitOfWork;
            UserRepository = userRepository;
            AddressRepository = addressRepository;
            AddressTypeRepository = addressTypeRepository;
        }

        #endregion ctor

        #region Public methods

        public async Task<AddressAssignmentDto> Handle(
            AssignAddressToUserCommand command
            , CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrEmpty(command.AddressTypeName))
                throw new ArgumentNullException(nameof(command.AddressTypeName));
            if (command.UserId == Guid.Empty) throw new ArgumentNullException(nameof(command.UserId));
            if (command.HouseNumber <= 0) throw new ArgumentNullException(nameof(command.HouseNumber));
            if (string.IsNullOrEmpty(command.Line1)) throw new ArgumentNullException(nameof(command.HouseNumber));
            if (string.IsNullOrEmpty(command.CountryName)) throw new ArgumentNullException(nameof(command.HouseNumber));
            if (string.IsNullOrEmpty(command.TownName)) throw new ArgumentNullException(nameof(command.HouseNumber));
            if (string.IsNullOrEmpty(command.PostalCode)) throw new ArgumentNullException(nameof(command.HouseNumber));

            var user = await UserRepository
                .Queryable()
                .Where(user => user.Id == command.UserId)
                .SingleOrDefaultAsync(cancellationToken);

            if (user == null)
                throw new DomainException("Application user not found by requested Id of: [ " + command.Id + " ]");

            var addressTypeNameSanitized = command.AddressTypeName.Trim().ToUpper();

            var addressType = await AddressTypeRepository
                .Queryable()
                .Where(addressT => addressT.Name.Trim().ToUpper() == addressTypeNameSanitized)
                .SingleOrDefaultAsync(cancellationToken);

            if (addressType == null)
                throw new DomainException("Address type not found by requested name of: [ " + command.AddressTypeName +
                                          " ]");

            if (command.AssignerUser == null)
            {
                command.AssignerUser = await UserRepository.Queryable().Where(auser =>
                        auser.Id == null)
                    .SingleOrDefaultAsync(cancellationToken);
                if (command.AssignerUser == null)
                    throw new DomainException(
                        "Cannot find assigner user when assigning an address to user, cannot continue.");
            }

            var address = Address.NewActiveDraft(
                command.AddressName
                , command.Description
                , command.Line1
                , command.Line2
                , command.FlatNr
                , command.PostalCode
                , command.HouseNumber
                , command.HouseNumberSuffix
                , command.UserComment
                , addressType
                , command.CityBlockName
                , command.CountryName
                , command.TownName
                , command.CountyName
                , command.AssignerUser
                , command.DateCreated
                , command.ActiveFrom
                , command.ActiveTo
            );

            user.AssignAddress(address, addressType, user);

            UserRepository.Update(user);

            var res = await UnitOfWork.SaveChangesAsync(cancellationToken);

            return new AddressAssignmentDto
            {
                Line1 = address.Line1,
                Line2 = address.Line2,
                CityBlock = address.CityBlock,
                Country = address.Country,
                County = address.County,
                FlatNr = address.FlatNr,
                HouseNumber = address.HouseNumber,
                HouseNumberSuffix = address.HouseNumberSuffix,
                Town = address.Town,
                PostalCode = address.PostalCode,
                UserComment = address.UserComment,
                AsigneeUserName = user.UserName
            };
        }

        #endregion Public methods

        #region Private properties

        private ITrackableRepository<AddressType> AddressTypeRepository { get; }
        private ITrackableRepository<Address> AddressRepository { get; }
        private IMyUnitOfWork UnitOfWork { get; }
        private ITrackableRepository<User> UserRepository { get; }

        #endregion Private properties
    }
}