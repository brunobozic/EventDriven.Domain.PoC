using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EventDriven.Domain.PoC.Domain.DomainEntities.DomainExceptions;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AccountJournal;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate.AddressDomainEvents;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.EmailSending;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.PasswordReset;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.Verification;
using EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces;
using EventDriven.Domain.PoC.SharedKernel.Helpers;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Random;
using Microsoft.Extensions.Options;
using TrackableEntities.Common.Core;
using BC = BCrypt.Net.BCrypt;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate
{
    public class User : BasicDomainEntity<long>, IAuditTrail
    {
        private const int NumYearsDefaultActivity = 1;

        #region Email templates

        public bool SendAlreadyRegisteredEmail(string origin)
        {
            var message = !string.IsNullOrEmpty(origin)
                ? $@"<p>If you don't know your password please visit the <a href=""{origin}/applicationUser/forgot-password"">forgot password</a> page.</p>"
                : "<p>If you don't know your password you can reset it via the <code>/applicationUsers/forgot-password</code> api route.</p>";

            var emailSubject = "Sign-up Verification API - Email Already Registered";
            var completeEmailMessageBody = $@"<h4>Email Already Registered</h4>
                         <p>Your email <strong>{Email}</strong> is already registered.</p>
                         {message}";
            var emailFrom = "admin.poc@gmail.com";


            // when this domain event gets handled [AccountAlreadyRegisteredMailReadiedDomainEventHandler], a new command [AccountAlreadyRegisteredMailCommand]
            // will be created that will actually do the email sending
            AddDomainEvent(new AccountAlreadyRegisteredMailReadiedDomainEvent(
                Email
                , UserName
                , Id
                , emailSubject
                , completeEmailMessageBody
                , emailFrom
            ));

            return true;
        }

        #endregion Email templates

        #region Public Properties

        [EnumDataType(typeof(RoleEnum))] public string BasicRole { get; private set; }

        public string FullName { get; private set; }
        public string Oib { get; private set; }
        public DateTimeOffset? DateOfBirth { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string UserName { get; private set; }
        public string NormalizedEmail { get; private set; }
        public string NormalizedUserName { get; private set; }
        public bool TwoFactorEnabled { get; private set; }
        public DateTime Verified { get; private set; }
        public string VerificationFailureLatestMessage { get; private set; }
        public DateTime LastVerificationFailureDate { get; private set; }

        private RegistrationStatusEnum _status { get; set; }


        #region Token operation

        public string ResetToken { get; private set; }
        public DateTime? ResetTokenExpires { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime? PasswordReset { get; private set; }
        public string VerificationToken { get; private set; }

        public DateTime? VerificationTokenExpirationDate { get; private set; }

        //public RegistrationStatusEnum Status { get; private set; }
        public Guid UserIdGuid { get; private set; }

        #endregion Token operations

        public DateTime Created { get; private set; }
        public DateTime Updated { get; private set; }
        public string PasswordResetMsg { get; private set; }

        #endregion Public Properties

        #region Ctor

        /// <summary>
        ///     For EFCore
        /// </summary>
        private User()
        {
            _refreshTokens = new List<RefreshToken.RefreshToken>();
            _userRoles = new List<UserRole>();
            _journalEntries = new List<AccountJournalEntry>();
            _userAddresses = new List<UserAddress>();
        }

        /// <summary>
        ///     Returns a new untracked draft of an [ApplicationUser], with [Active] pre-set to false, no password and no email
        ///     confirmed
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="role"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static User NewDraft(
            string email
            , string userName
            , string firstName
            , string lastName
            , string password
            , string role
            , User creator
        )
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));
            if (string.IsNullOrEmpty(firstName)) throw new ArgumentNullException(nameof(firstName));
            if (string.IsNullOrEmpty(lastName)) throw new ArgumentNullException(nameof(lastName));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            var userIdGuid = Guid.NewGuid();

            var user = new User
            {
                Email = email.Trim(),
                UserName = userName,
                NormalizedEmail = email.Trim().ToUpper(),
                NormalizedUserName = userName.Trim().ToUpper(),
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                FullName = lastName.Trim() + " " + firstName.Trim(),
                TwoFactorEnabled = false,
                Created = DateTime.UtcNow,
                _status = RegistrationStatusEnum.WaitingForVerification,
                Oib = "",
                UserIdGuid = userIdGuid
            };

            user.AddPasswordHash(password);
            user.AddVerificationToken(RandomStringHelper.RandomTokenString());

            user.AddDomainEvent(new UserCreatedDomainEvent(
                email
                , userName
                , firstName
                , lastName
                , userIdGuid
                , ""
                , DateTimeOffset.MinValue
                , DateTimeOffset.MinValue
                , ""
                , creator.Id
            ));

            return user;
        }

        /// <summary>
        ///     Returns a new untracked [ApplicationUser], with [Active] pre-set to true, password set and no email confirmed
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="oib"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="activeFrom"></param>
        /// <param name="activeTo"></param>
        /// <param name="password"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static User NewActiveWithPassword(
            string email
            , string userName
            , string firstName
            , string lastName
            , string oib
            , DateTimeOffset? dateOfBirth
            , DateTimeOffset activeFrom
            , DateTimeOffset activeTo
            , string password
            , User creator
        )
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));
            if (string.IsNullOrEmpty(firstName)) throw new ArgumentNullException(nameof(firstName));
            if (string.IsNullOrEmpty(lastName)) throw new ArgumentNullException(nameof(lastName));
            if (string.IsNullOrEmpty(oib)) throw new ArgumentNullException(nameof(oib));
            if (dateOfBirth >= DateTimeOffset.UtcNow) throw new ArgumentNullException(nameof(dateOfBirth));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            if (activeFrom == DateTimeOffset.MinValue) activeFrom = DateTimeOffset.UtcNow;
            if (activeTo == DateTimeOffset.MinValue) activeTo = DateTimeOffset.UtcNow.AddYears(NumYearsDefaultActivity);

            var userIdGuid = Guid.NewGuid();

            var user = new User
            {
                Email = email.Trim(),
                UserName = userName,
                NormalizedEmail = email.Trim().ToUpper(),
                NormalizedUserName = userName.Trim().ToUpper(),
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                FullName = lastName.Trim() + " " + firstName.Trim(),
                TwoFactorEnabled = false,
                _status = RegistrationStatusEnum.WaitingForVerification,
                Created = DateTime.UtcNow,
                TrackingState = TrackingState.Added,
                Oib = oib,
                DateOfBirth = dateOfBirth,
                UserIdGuid = userIdGuid
            };

            user.Activate(activeFrom, activeTo, creator);

            user.AddPasswordHash(password);
            user.AddVerificationToken(RandomStringHelper.RandomTokenString());

            user.AddDomainEvent(new UserCreatedDomainEvent(
                email
                , userName
                , firstName
                , lastName
                , userIdGuid
                , oib
                , dateOfBirth
                , DateTime.UtcNow
                , user.VerificationToken
                , creator.Id
            ));

            return user;
        }


        /// <summary>
        ///     Returns a new untracked [ApplicationUser], with [Active] pre-set to true, password set and email already confirmed
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="oib"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="activeFrom"></param>
        /// <param name="activeTo"></param>
        /// <param name="password"></param>
        /// <param name="activator"></param>
        /// <returns></returns>
        public static User NewActiveWithPasswordAndEmailVerified(
            string email
            , string userName
            , string firstName
            , string lastName
            , string oib
            , DateTimeOffset? dateOfBirth
            , DateTimeOffset activeFrom
            , DateTimeOffset activeTo
            , string password
            , User activator = null
        )
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));
            if (string.IsNullOrEmpty(firstName)) throw new ArgumentNullException(nameof(firstName));
            if (string.IsNullOrEmpty(lastName)) throw new ArgumentNullException(nameof(lastName));
            if (string.IsNullOrEmpty(oib)) throw new ArgumentNullException(nameof(oib));
            if (dateOfBirth >= DateTimeOffset.UtcNow) throw new ArgumentNullException(nameof(dateOfBirth));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            if (activeFrom == DateTimeOffset.MinValue) activeFrom = DateTimeOffset.UtcNow;
            if (activeTo == DateTimeOffset.MinValue) activeTo = DateTimeOffset.UtcNow.AddYears(NumYearsDefaultActivity);

            var userIdGuid = Guid.NewGuid();

            var user = new User
            {
                Email = email.Trim(),
                UserName = userName,
                NormalizedEmail = email.Trim().ToUpper(),
                NormalizedUserName = userName.Trim().ToUpper(),
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                FullName = lastName.Trim() + " " + firstName.Trim(),
                _status = RegistrationStatusEnum.Verified,
                TwoFactorEnabled = false,
                Created = DateTime.UtcNow,
                TrackingState = TrackingState.Added,
                Oib = oib,
                DateOfBirth = dateOfBirth,
                UserIdGuid = userIdGuid
            };

            if (activator == null)
                user.ActivateWithNoActivator(activeFrom, activeTo);
            else
                user.Activate(activeFrom, activeTo, activator);

            user.AddPasswordHash(password);
            user.AddVerificationToken(RandomStringHelper.RandomTokenString());
            user.SetEmailIsVerified();

            if (activator != null)
                user.AddDomainEvent(new UserCreatedDomainEvent(
                    email
                    , userName
                    , firstName
                    , lastName
                    , userIdGuid
                    , oib
                    , dateOfBirth
                    , DateTime.UtcNow
                    , user.VerificationToken
                    , activator.Id
                ));
            else // seeding related issue
                user.AddDomainEvent(new UserCreatedDomainEvent(
                    email
                    , userName
                    , firstName
                    , lastName
                    , userIdGuid
                    , oib
                    , dateOfBirth
                    , DateTime.UtcNow
                    , user.VerificationToken
                    , 1 // <== this will happen when seeding
                ));

            return user;
        }

        #endregion Ctor

        #region FK

        public long? UndeletedById { get; private set; }
        public long? ReactivatedById { get; private set; }

        public long? DeactivatedById { get; private set; }
        //public long? ActivatedById { get; private set; }
        //public long? DeletedById { get; private set; }

        #endregion FK

        #region Navigation properties

        private readonly List<UserRole> _userRoles;
        public IReadOnlyCollection<UserRole> UserRoles => _userRoles;

        private readonly List<AccountJournalEntry> _journalEntries;
        public IReadOnlyCollection<AccountJournalEntry> JournalEntries => _journalEntries;

        public IOptions<MyConfigurationValues> _appSettings { get; }

        private readonly List<RefreshToken.RefreshToken> _refreshTokens;
        public IReadOnlyCollection<RefreshToken.RefreshToken> RefreshTokens => _refreshTokens;
        private readonly List<UserAddress> _userAddresses;
        public IReadOnlyCollection<UserAddress> UserAddresses => _userAddresses;

        #endregion Navigation properties

        #region Public methods

        #region Addresses

        // TODO: transform to event driven
        public bool AssignAddress(Address address, AddressType addressType, User addressAssigner)
        {
            if (EnsureIsActive() && !Deleted)
            {
                var newAddress = address;
                newAddress.AssignAddressType(addressType, addressAssigner);

                var userAddress = UserAddress.NewActivatedDraft(this, address, addressAssigner);

                _userAddresses.Add(userAddress);

                Updated = DateTime.UtcNow;

                var journalEntry = new AccountJournalEntry(DateTime.UtcNow + " => [" + addressType.Name +
                                                           "] address assigned to user. Address assigned: [" +
                                                           address.Line1 + "].");
                journalEntry.AttachActingUser(addressAssigner);
                journalEntry.AttachUser(this);
                _journalEntries.Add(journalEntry);

                AddDomainEvent(new AddressAssignedToUserDomainEvent(Id, newAddress, addressAssigner.Id,
                    DateTimeOffset.UtcNow));

                return true;
            }

            throw new DomainException(
                "Entity is either already deactivated or deleted, hence we are unable to add an address to it.");
        }

        // TODO: transform to event driven
        public bool RemoveAddressFromUser(Address addressToRemove, AddressType addressType, User addressRemover)
        {
            if (EnsureIsActive() && !Deleted)
            {
                var userAddressIdsToDelete = _userAddresses
                    .Where(userAddress =>
                        !userAddress.Deleted && userAddress.Address.AddressIdGuid == addressToRemove.AddressIdGuid &&
                        !userAddress.Address.Deleted)
                    .Select(uid => uid.Id)
                    .ToList();


                _userAddresses.RemoveAll(userAddress => userAddressIdsToDelete.Contains(userAddress.Id));

                Updated = DateTime.UtcNow;

                var journalEntry = new AccountJournalEntry(DateTime.UtcNow + " => [" + addressType.Name +
                                                           "] address removed from user. Address removed: [" +
                                                           addressToRemove.Line1 + "].");
                journalEntry.AttachActingUser(addressRemover);
                journalEntry.AttachUser(this);
                _journalEntries.Add(journalEntry);

                AddDomainEvent(new AddressRemovedFromUserDomainEvent(Id, addressToRemove, addressRemover.Id,
                    DateTimeOffset.UtcNow));

                return true;
            }

            throw new DomainException(
                "Entity is either already deactivated or deleted, hence we are unable to add an address to it.");
        }

        #endregion Addresses

        /// <summary>
        ///     Returns the date time expiry value of the entity
        /// </summary>
        /// <param name="theDate"></param>
        /// <returns></returns>
        public virtual bool IsExpired(DateTimeOffset theDate)
        {
            return ActiveTo < theDate;
        }

        // TODO: transform to event driven
        public bool RevokeToken(string token, string ipAddress, User revokerUser)
        {
            var refreshTokenDomain =
                _refreshTokens.SingleOrDefault(tok => tok.Token.Trim().ToUpper() == token.Trim().ToUpper());
            refreshTokenDomain.Revoked = DateTime.UtcNow;
            refreshTokenDomain.RevokedByIp = ipAddress;

            var journalEntry = new AccountJournalEntry(DateTime.UtcNow + " => Token revoked.");
            journalEntry.AttachActingUser(revokerUser);
            journalEntry.AttachUser(this);
            _journalEntries.Add(journalEntry);

            return true;
        }

        public virtual bool IsDeleted()
        {
            return Deleted;
        }

        public virtual bool IsDeactivated()
        {
            return !Active;
        }

        /// <summary>
        ///     Returns the activity validity of the entity
        /// </summary>
        /// <returns></returns>
        private bool EnsureIsActive()
        {
            return ActiveTo >= DateTimeOffset.UtcNow;
        }

        // TODO: transform to event driven
        /// <summary>
        ///     Activates the entity, may throw a domain exception
        /// </summary>
        public new bool Activate(DateTimeOffset from, DateTimeOffset to, User activatedBy)
        {
            if (!EnsureIsActive() && !Deleted)
                try
                {
                    base.Activate(from, to, activatedBy);

                    var journalEntry = activatedBy != null
                        ? new AccountJournalEntry(DateTime.UtcNow + " => Activated by [ " + activatedBy.UserName +
                                                  " ] .")
                        : new AccountJournalEntry(DateTime.UtcNow + " => Activated by [ System user (unknown) ] .");
                    journalEntry.AttachActingUser(activatedBy);
                    journalEntry.AttachUser(this);
                    _journalEntries.Add(journalEntry);

                    return true;
                }
                catch (Exception ex)
                {
                    throw new DomainException("System error activating the entity.", ex);
                }

            throw new DomainException(
                "Entity is either already active or deleted, hence we are unable to activate it.");
        }

        // TODO: transform to event driven
        /// <summary>
        ///     Deactivates the entity, may throw a domain exception
        /// </summary>
        public bool Deactivate(DateTimeOffset from, DateTimeOffset to, User deactivatedBy, string reason)
        {
            if (EnsureIsActive() && !Deleted)
                try
                {
                    base.Deactivate(deactivatedBy, reason);

                    var journalEntry = new AccountJournalEntry(DateTime.UtcNow + " => Deactivated by [ " +
                                                               DeactivatedById + " ] : " + reason);
                    journalEntry.AttachActingUser(deactivatedBy);
                    journalEntry.AttachUser(this);

                    _journalEntries.Add(journalEntry);

                    return true;
                }
                catch (Exception ex)
                {
                    throw new DomainException("System error deactivating the entity.", ex);
                }

            throw new DomainException(
                "Entity is either already deactivated or deleted, hence we are unable to deactivate it.");
        }

        #region Roles

        public List<Role> GetUserRoles()
        {
            return _userRoles.Where(userRole => userRole.Active).Select(role => role.Role).ToList();
        }

        public bool AddRole(Role role, User roleGiver)
        {
            if (EnsureIsActive() && !Deleted)
            {
                if (!role.IsExpired(DateTime.Now) && !role.IsDeleted())
                {
                    var newJoin = UserRole.NewActivatedDraft(this, role, roleGiver);

                    _userRoles.Add(newJoin);

                    // this will get audited, but will not show what had changed (audit works on per-table basis, not on object-graph basis)
                    Updated = DateTime.UtcNow;

                    AddDomainEvent(new RoleAssignedToUserDomainEvent(
                        Id
                        , UserName
                        , Email
                        , role.Id
                        , role.Name
                        , role.ActiveTo
                        , roleGiver.Id
                        , roleGiver.Email
                        , roleGiver.UserName
                        , DateTimeOffset.UtcNow
                    ));

                    return true;
                }

                throw new DomainException(
                    "The role entity is either expired or deleted, hence we are unable to add it to the user.");
            }

            throw new DomainException(
                "The user entity is either already deactivated or deleted, hence we are unable to add a role to it.");
        }

        public bool RemoveRole(Role role, User removerUser)
        {
            if (EnsureIsActive() && !Deleted)
            {
                if (!role.IsExpired(DateTime.Now) && !role.IsDeleted())
                {
                    var linkTableEntry =
                        _userRoles.SingleOrDefault(role =>
                            role.Active && role.Role.Name.Trim().ToUpper() == role.Name);

                    if (linkTableEntry == null)
                        throw new AggregateException(
                            "Could not find an entry in the join table by the role name of: [ " +
                            role.Name + " ]");

                    _userRoles.Remove(linkTableEntry);

                    // this will get audited, but will not show what had changed (audit works on per-table basis, not on object-graph basis)
                    Updated = DateTime.UtcNow;

                    AddDomainEvent(new RoleRemovedFromUserDomainEvent(
                        Id,
                        UserName,
                        Email,
                        role.Id,
                        role.Name,
                        removerUser.Id,
                        removerUser.UserName,
                        removerUser.Email,
                        DateTimeOffset.UtcNow
                    ));

                    return true;
                }

                throw new DomainException(
                    "The role entity is either expired or deleted, hence we are unable to remove it from the user.");
            }

            throw new DomainException(
                "Entity is either already deactivated or deleted, hence we are unable to remove a role from it.");
        }

        #endregion Roles

        public bool SetEmailIsVerified()
        {
            if (EnsureIsActive() && !Deleted && VerificationTokenExpirationDate >= DateTime.UtcNow)
            {
                Verified = DateTime.UtcNow;
                VerificationToken = null;
                VerificationTokenExpirationDate = null;
                _status = RegistrationStatusEnum.Verified;
                AddDomainEvent(new EmailVerifiedDomainEvent(Email, UserName, Id));

                return true;
            }

            if (!EnsureIsActive())
                VerificationFailureLatestMessage = "Unable to verify user account, the account is inactive." + ", ";
            if (Deleted)
                VerificationFailureLatestMessage += "Unable to verify user account, the account is deleted." + ", ";
            if (VerificationTokenExpirationDate <= DateTime.UtcNow)
                VerificationFailureLatestMessage +=
                    "Unable to verify user account, the account verification URL has expired.";
            LastVerificationFailureDate = DateTime.UtcNow;
            _status = RegistrationStatusEnum.VerificationFailed;

            AddDomainEvent(new EmailNotVerifiedDomainEvent(Email, UserName, Id, VerificationFailureLatestMessage));

            return false;
        }

        public bool AddPasswordHash(string password)
        {
            if (EnsureIsActive() && !Deleted)
            {
                PasswordHash = BC.HashPassword(password);
                return true;
            }

            throw new DomainException(
                "Entity is either already deactivated or deleted, hence we are unable to add a password to it.");
        }

        // TODO: transform to event driven
        public bool CreatePasswordResetToken(string randomToken)
        {
            if (EnsureIsActive() && !Deleted)
            {
                ResetToken = randomToken;
                ResetTokenExpires = DateTime.UtcNow.AddHours(8);

                var journalEntry = new AccountJournalEntry(DateTime.UtcNow + " => Reset password token created.");

                journalEntry.AttachActingUser(null);
                journalEntry.AttachUser(this);

                _journalEntries.Add(journalEntry);
            }
            else
            {
                throw new DomainException(
                    "Entity is either already deactivated or deleted, hence we are unable to create a password reset token for it.");
            }

            AddDomainEvent(new UserRequestedPasswordResetDomainEvent(Email, UserName, Id, randomToken));

            return true;
        }

        public bool AddVerificationToken(string verificationToken)
        {
            if (EnsureIsActive() && !Deleted)
            {
                VerificationToken = verificationToken;
                VerificationTokenExpirationDate = DateTime.UtcNow.AddHours(8);
            }
            else
            {
                throw new DomainException(
                    "Entity is either already deactivated or deleted, hence we are unable to add a verification token to it.");
            }

            return true;
        }

        public bool RemoveStaleRefreshTokens()
        {
            if (EnsureIsActive() && !Deleted)
                _refreshTokens.RemoveAll(x => x.Created.AddDays(_appSettings.Value.RefreshTokenTTL) <= DateTime.UtcNow);
            else
                throw new DomainException(
                    "Entity is either already deactivated or deleted, hence we are unable to remove stale refresh tokens from it.");

            return true;
        }

        public bool AddRefreshToken(RefreshToken.RefreshToken model)
        {
            if (EnsureIsActive() && !Deleted)
                _refreshTokens.Add(model);
            else
                throw new DomainException(
                    "Entity is either already deactivated or deleted, hence we are unable to add a refresh token to it.");

            return true;
        }

        public List<RefreshToken.RefreshToken> GetRefreshTokens()
        {
            //return _refreshTokens.Where(token => token.IsActive && token.ApplicationUser.Id == this.Id && token.ApplicationUser.Active).ToList();
            if (EnsureIsActive() && !Deleted)
                return _refreshTokens.ToList();
            throw new DomainException(
                "Entity is either already deactivated or deleted, hence we are unable to fetch refresh tokens for it.");
        }

        // TODO: transform to event driven
        public bool UpdatePasswordRemoveResetToken(string password)
        {
            if (EnsureIsActive() && !Deleted)
            {
                var hashedPassword = BC.HashPassword(password);
                PasswordHash = hashedPassword;
                PasswordReset = DateTime.UtcNow;
                ResetToken = null;
                ResetTokenExpires = null;
            }
            else
            {
                throw new DomainException(
                    "Entity is either already deactivated or deleted, hence we are unable to update password and remove reset token for it.");
            }

            AddDomainEvent(new PasswordResetCompletedDomainEvent(Email, UserName, Id));

            return true;
        }

        public bool OwnsToken(string token)
        {
            if (EnsureIsActive() && !Deleted)
                return _refreshTokens?.Find(x => x.Token == token) != null;
            throw new DomainException(
                "Entity is either already deactivated or deleted, hence we are unable to fetch a token to it.");
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        public RegistrationStatusEnum GetStatus()
        {
            return _status;
        }

        #endregion Public methods
    }
}