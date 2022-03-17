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
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TrackableEntities.Common.Core;
using BC = BCrypt.Net.BCrypt;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate
{
    public class User : BasicDomainEntity<Guid>, IAuditTrail, IAggregateRoot
    {
        private const int NumYearsDefaultActivity = 1;

        public void AccountActivationMailNotSent(string eMessage)
        {
            _accountActivationMailsSendAttempts += 1;
            var journalEntry =
                new AccountJournalEntry(DateTime.UtcNow +
                                        " => Exception -> unable to send account activation code sent to users e-mail address.");
            journalEntry.AttachActingUser(null);
            journalEntry.AttachUser(this);
            _journalEntries.Add(journalEntry);
            _status = RegistrationStatusEnum.VerificationEmailSent;
            _latestVerificationFailureMessage = eMessage;
            _latestVerificationFailureTime = DateTime.UtcNow;
        }


        #region Public Properties

        //[EnumDataType(typeof(RoleEnum))] public string BasicRole { get; private set; }

        public string FullName { get; private init; }
        public string Oib { get; private init; }
        public DateTimeOffset? DateOfBirth { get; private init; }
        public string FirstName { get; private init; }
        public string LastName { get; private init; }
        public string Email { get; private init; }
        public string UserName { get; private init; }
        public string NormalizedEmail { get; private init; }
        public string NormalizedUserName { get; private init; }
        public bool TwoFactorEnabled { get; private init; }

        private int _accountActivationMailsSendAttempts { get; set; }
        private string _latestVerificationFailureMessage { get; set; }
        private DateTime _latestVerificationFailureTime { get; set; }

        #region Email Verification

        public DateTime Verified { get; private set; }
        public string LatestVerificationFailureMessage { get; private set; }
        public DateTime LastVerificationFailureDate { get; private set; }

        private RegistrationStatusEnum _status { get; set; }

        #endregion Email Verification

        #region Token operations

        public string ResetToken { get; private set; }
        public string PasswordResetMsg { get; private set; }

        public DateTime? ResetTokenExpires { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime? PasswordReset { get; private set; }
        public string EmailVerificationToken { get; private set; }

        public DateTime? VerificationTokenExpirationDate { get; private set; }

        #endregion Token operations

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
        ///     Returns a new un-tracked draft of an [ApplicationUser], with [Active] pre-set to false, no password and no email
        ///     confirmed
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <param name="creator"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static User NewDraft(
            Guid userId
            , string email
            , string userName
            , string firstName
            , string lastName
            , string password
            , string role
            , User creator
            , string origin
        )
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));
            if (string.IsNullOrEmpty(firstName)) throw new ArgumentNullException(nameof(firstName));
            if (string.IsNullOrEmpty(lastName)) throw new ArgumentNullException(nameof(lastName));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));

            var user = new User
            {
                Id = userId,
                Email = email.Trim(),
                UserName = userName,
                NormalizedEmail = email.Trim().ToUpper(),
                NormalizedUserName = userName.Trim().ToUpper(),
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                FullName = lastName.Trim() + " " + firstName.Trim(),
                TwoFactorEnabled = false,
                DateCreated = DateTime.UtcNow,
                _status = RegistrationStatusEnum.WaitingForVerification,
                Oib = ""
            };

            user.AddPasswordHash(password);
            user.AddVerificationToken(RandomStringHelper.RandomTokenString());

            user.AddDomainEvent(new UserCreatedDomainEvent(
                userId
                , email
                , userName
                , firstName
                , lastName
                , ""
                , DateTimeOffset.MinValue
                , DateTimeOffset.MinValue
                , ""
                , creator.Id
                , origin
            ));

            return user;
        }

        /// <summary>
        ///     Returns a new un-tracked [ApplicationUser], with [Active] pre-set to true, password set and no email confirmed
        /// </summary>
        /// <param name="userId"></param>
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
        /// <param name="origin"></param>
        /// <returns></returns>
        public static User NewActiveWithPassword(
            Guid userId
            , string email
            , string userName
            , string firstName
            , string lastName
            , string oib
            , DateTimeOffset? dateOfBirth
            , DateTimeOffset activeFrom
            , DateTimeOffset activeTo
            , string password
            , User creator
            , string origin
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
                DateCreated = DateTime.UtcNow,
                TrackingState = TrackingState.Added,
                Oib = oib,
                DateOfBirth = dateOfBirth
            };

            user.Activate(activeFrom, activeTo, creator);

            user.AddPasswordHash(password);
            user.AddVerificationToken(RandomStringHelper.RandomTokenString());

            var creatorId = creator?.Id;

            user.AddDomainEvent(new UserCreatedDomainEvent(
                userId
                , email
                , userName
                , firstName
                , lastName
                , oib
                , dateOfBirth
                , DateTime.UtcNow
                , user.EmailVerificationToken
                , creatorId
                , origin
            ));

            return user;
        }

        /// <summary>
        ///     Returns a new un-tracked [ApplicationUser], with [Active] pre-set to true, password set and email already confirmed
        /// </summary>
        /// <param name="userId"></param>
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
        /// <param name="origin"></param>
        /// <param name="isSeed"></param>
        /// <returns></returns>
        public static User NewActiveWithPasswordAndEmailVerified(
            Guid userId
            , string email
            , string userName
            , string firstName
            , string lastName
            , string oib
            , DateTimeOffset? dateOfBirth
            , DateTimeOffset activeFrom
            , DateTimeOffset activeTo
            , string password
            , User activator
            , string origin
            , bool isSeed
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

            var user = new User
            {
                Id = userId,
                Email = email.Trim(),
                UserName = userName,
                NormalizedEmail = email.Trim().ToUpper(),
                NormalizedUserName = userName.Trim().ToUpper(),
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                FullName = lastName.Trim() + " " + firstName.Trim(),
                _status = RegistrationStatusEnum.Verified,
                TwoFactorEnabled = false,
                DateCreated = DateTime.UtcNow,
                TrackingState = TrackingState.Added,
                Oib = oib,
                DateOfBirth = dateOfBirth,
                IsSeed = isSeed
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
                    userId
                    , email
                    , userName
                    , firstName
                    , lastName
                    , oib
                    , dateOfBirth
                    , DateTime.UtcNow
                    , user.EmailVerificationToken
                    , activator.Id
                    , origin
                ));
            else // seeding related issue
                user.AddDomainEvent(new UserCreatedDomainEvent(
                    userId
                    , email
                    , userName
                    , firstName
                    , lastName
                    , oib
                    , dateOfBirth
                    , DateTime.UtcNow
                    , user.EmailVerificationToken
                    , null // <== this will happen when seeding, the first user inserted (via seeding) does not have a [User] entity as its own creator
                    , origin
                ));

            return user;
        }

        #endregion Ctor

        #region FK

        public Guid? UndeletedById { get; private set; }
        public Guid? ReactivatedById { get; private set; }
        public Guid? DeactivatedById { get; private set; }

        #endregion FK

        #region Navigation properties

        private readonly List<UserRole> _userRoles;
        public IReadOnlyCollection<UserRole> UserRoles => _userRoles;

        private readonly List<AccountJournalEntry> _journalEntries;
        public IReadOnlyCollection<AccountJournalEntry> JournalEntries => _journalEntries;

        private readonly List<RefreshToken.RefreshToken> _refreshTokens;
        public IReadOnlyCollection<RefreshToken.RefreshToken> RefreshTokens => _refreshTokens;
        private readonly List<UserAddress> _userAddresses;

        public IReadOnlyCollection<UserAddress> UserAddresses => _userAddresses;

        #endregion Navigation properties

        #region Public methods

        #region Addresses

        public bool AssignAddress(Address address, AddressType addressType, User addressAssigner)
        {
            if (!TheUserIsInActiveState() || !TheUserHadBeenVerified() || IsDeleted)
                throw new DomainException(
                    $"The user [{UserName} / {Email}] is either deactivated, deleted or has not yet verified his account, hence we are unable to add an address to it. Current registration status: {GetCurrentRegistrationStatus()}");
            var newAddress = address;
            newAddress.AssignAddressType(addressType, addressAssigner);

            var userAddress = UserAddress.NewActivatedDraft(this, address, addressAssigner);

            _userAddresses.Add(userAddress);

            DateModified = DateTime.UtcNow;

            AddDomainEvent(new AddressAssignedToUserDomainEvent(
                this.Id
                , this.UserName
                , this.Email
                , address.Id
                , address.Line1
                , address.Active
                , address.AddressIdGuid
                , address.HouseNumber
                , address.HouseNumberSuffix
                , address.FlatNr
                , address.Town.Id
                , address.Town.Name
                , address.Town.DateCreated
                , address.Town.DateModified
                , address.Town.ZipCode
                , address.PostalCode
                , address.DateCreated
                , address.DateModified
                , addressType.Id
                , addressType.Name
                , addressType.Active
                , addressType.Description
                , addressAssigner.Id
                , addressAssigner.UserName
                , addressAssigner.Email,
                DateTimeOffset.UtcNow));

            return true;
        }

        public bool RemoveAddressFromUser(Address addressToRemove, AddressType addressType, User addressRemover)
        {
            if (!TheUserIsInActiveState() || !TheUserHadBeenVerified() || IsDeleted)
                throw new DomainException(
                    $"The user [{UserName} / {Email}] is either deactivated, deleted or has not yet verified his account, hence we are unable to add an address to it. Current registration status: {GetCurrentRegistrationStatus()}");
            var userAddressIdsToDelete = _userAddresses
                .Where(userAddress =>
                    !userAddress.TheAddressHasBeenDeleted() &&
                    userAddress.Address.AddressIdGuid == addressToRemove.AddressIdGuid &&
                    !userAddress.Address.TheAddressHasBeenDeleted())
                .Select(uid => uid.Id)
                .ToList();

            _userAddresses.RemoveAll(userAddress => userAddressIdsToDelete.Contains(userAddress.Id));

            DateModified = DateTime.UtcNow;

            AddDomainEvent(new AddressRemovedFromUserDomainEvent(
                this.Id
                , this.UserName
                , this.Email
                , addressToRemove.Id
                , addressToRemove.Line1
                , addressToRemove.Active
                , addressToRemove.AddressIdGuid
                , addressToRemove.HouseNumber
                , addressToRemove.HouseNumberSuffix
                , addressToRemove.FlatNr
                , addressToRemove.Town.Id
                , addressToRemove.Town.Name
                , addressToRemove.Town.DateCreated
                , addressToRemove.Town.DateModified
                , addressToRemove.Town.ZipCode
                , addressToRemove.PostalCode
                , addressToRemove.DateCreated
                , addressToRemove.DateModified
                , addressType.Id
                , addressType.Name
                , addressType.Active
                , addressType.Description
                , addressRemover.Id
                , addressRemover.UserName
                , addressRemover.Email,
                DateTimeOffset.UtcNow));

            return true;
        }

        #endregion Addresses

        public bool VerifyPasswordResetTokenThenResetPassword(string tokenReceivedByEmail, string newPassword)
        {
            if (string.IsNullOrEmpty(tokenReceivedByEmail))
                throw new DomainException(
                    $"The token received by email must not be null. Unable to confirm password reset for user [ {UserName} ], email [ {Email} ].");
            if (string.IsNullOrEmpty(newPassword))
                throw new DomainException(
                    $"The new password must not be null. Unable to confirm password reset for user [ {UserName} ], email [ {Email} ].");

            return ResetToken.Trim() == tokenReceivedByEmail.Trim() && UpdatePasswordThenRemoveResetToken(newPassword);
        }

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

        /// <summary>
        ///     Returns the date time expiry value of the entity
        /// </summary>
        /// <param name="theDate"></param>
        /// <returns></returns>
        public virtual bool IsExpired(DateTimeOffset theDate)
        {
            return ActiveTo < theDate;
        }

        public bool RevokeToken(string token, string ipAddress, User revokerUser)
        {
            var refreshTokenDomain =
                _refreshTokens.SingleOrDefault(tok =>
                    string.Equals(tok.Token.Trim(), token.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (refreshTokenDomain == null) return false;
            refreshTokenDomain.Revoked = DateTime.UtcNow;
            refreshTokenDomain.RevokedByIp = ipAddress;

            var journalEntry = new AccountJournalEntry(DateTime.UtcNow + " => Token revoked.");
            journalEntry.AttachActingUser(revokerUser);
            journalEntry.AttachUser(this);
            _journalEntries.Add(journalEntry);

            return true;
        }

        /// <summary>
        ///     Returns the activity validity of the entity
        /// </summary>
        /// <returns></returns>
        private bool TheUserIsInActiveState()
        {
            if (ActiveTo != null)
                return ActiveTo >= DateTimeOffset.UtcNow;

            return true;
        }

        private bool TheUserHadBeenVerified()
        {
            return Verified != DateTime.MinValue;
        }

        // TODO: transform to event driven
        /// <summary>
        ///     Activates the entity, may throw a domain exception
        /// </summary>
        public new bool Activate(DateTimeOffset from, DateTimeOffset to, User activatedBy)
        {
            if (!TheUserIsInActiveState() || IsDeleted)
                throw new DomainException(
                    $"The user [{UserName} / {Email}] is either deactivated [current activation status: {TheUserIsInActiveState()}], deleted [current: {IsDeleted}] or has not yet verified his account, hence we are unable to activate it. Current registration status: {GetCurrentRegistrationStatus()}");
            try
            {
                base.Activate(from, to, activatedBy);

                return true;
            }
            catch (Exception ex)
            {
                throw new DomainException(
                    $"System error activating the user [{UserName} / {Email}]. Current registration status: {GetCurrentRegistrationStatus()}",
                    ex);
            }
        }

        // TODO: transform to event driven
        /// <summary>
        ///     Deactivates the entity, may throw a domain exception
        /// </summary>
        public bool Deactivate(DateTimeOffset from, DateTimeOffset to, User deactivatedBy, string reason)
        {
            if (TheUserIsInActiveState() || IsDeleted)
                throw new DomainException(
                    $"The user [{UserName} / {Email}] is either deactivated, deleted or has not yet verified his account, hence we are unable to deactivate it. Current registration status: {GetCurrentRegistrationStatus()}");
            try
            {
                base.Deactivate(deactivatedBy, reason);

                return true;
            }
            catch (Exception ex)
            {
                throw new DomainException(
                    $"System error deactivating the user [{UserName} / {Email}]. Current registration status: {GetCurrentRegistrationStatus()}",
                    ex);
            }
        }

        #region Roles

        public List<Role> GetUserRoles()
        {
            return _userRoles.Where(userRole => userRole.Active).Select(role => role.Role).ToList();
        }

        public bool AddRole(Role role, User roleGiver)
        {
            if (!TheUserIsInActiveState() || !TheUserHadBeenVerified() || IsDeleted)
                throw new DomainException(
                    $"The user [{UserName} / {Email}] is either deactivated, deleted or has not yet verified his account, hence we are unable to add a role [ {role.Name} ] to it. Current registration status: {GetCurrentRegistrationStatus()}");
            if (role.IsExpired(DateTime.Now) || role.IsDeleted)
                throw new DomainException(
                    $"The role entity [ {role.Name} ] is either expired or deleted, hence we are unable to add it to the user.");
            var newJoin = UserRole.NewActivatedDraft(this, role, roleGiver);

            _userRoles.Add(newJoin);

            // this will get audited, but will not show what had changed (audit works on per-table basis, not on object-graph basis)
            DateModified = DateTime.UtcNow;

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

        public bool RemoveRole(Role role, User removerUser)
        {
            if (!TheUserIsInActiveState() || !TheUserHadBeenVerified() || IsDeleted)
                throw new DomainException(
                    $"The user [{UserName} / {Email}] is either deactivated, deleted or has not yet verified his account, hence we are unable to remove a role [ {role.Name} ] from it. Current registration status: {GetCurrentRegistrationStatus()}");
            if (role.IsExpired(DateTime.Now) || role.IsDeleted)
                throw new DomainException(
                    $"The role entity [ {role.Name} ] is either expired or deleted, hence we are unable to remove it from the user.");
            var linkTableEntry =
                _userRoles.SingleOrDefault(userRole =>
                    userRole.Active && userRole.Role.Name.Trim().ToUpper() == role.Name);

            if (linkTableEntry == null)
                throw new AggregateException(
                    "Could not find an entry in the join table by the role name of: [ " +
                    role.Name + " ]");

            _userRoles.Remove(linkTableEntry);

            // this will get audited, but will not show what had changed (audit works on per-table basis, not on object-graph basis)
            DateModified = DateTime.UtcNow;

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

        #endregion Roles

        public string SetEmailIsVerified()
        {
            // unfortunately I need a synchronous response here
            var friendlyErrorResponse = "OK";

            if (TheUserIsInActiveState() && !IsDeleted &&
                VerificationTokenExpirationDate >= DateTime.UtcNow)
            {
                Verified = DateTime.UtcNow;
                EmailVerificationToken = null;
                VerificationTokenExpirationDate = null;
                _status = RegistrationStatusEnum.Verified;

                AddDomainEvent(new EmailVerifiedDomainEvent(Email, UserName, Id, Verified));

                return friendlyErrorResponse;
            }

            friendlyErrorResponse = "";

            if (!TheUserIsInActiveState())
            {
                friendlyErrorResponse +=
                    $"Unable to verify user account [{UserName} / {Email}], the account is inactive. Current registration status: {GetCurrentRegistrationStatus()}" +
                    ", ";
                LatestVerificationFailureMessage = friendlyErrorResponse + ", ";
            }

            if (IsDeleted)
            {
                friendlyErrorResponse +=
                    $"Unable to verify user account [{UserName} / {Email}], the account is deleted. Current registration status: {GetCurrentRegistrationStatus()}" +
                    ", ";
                LatestVerificationFailureMessage += friendlyErrorResponse + ", ";
            }

            if (VerificationTokenExpirationDate <= DateTime.UtcNow)
            {
                friendlyErrorResponse +=
                    $"Unable to verify user account [{UserName} / {Email}], the account verification URL has expired. Current registration status: {GetCurrentRegistrationStatus()}" +
                    ", ";
                LatestVerificationFailureMessage += friendlyErrorResponse;
                LastVerificationFailureDate = DateTime.UtcNow;
                _status = RegistrationStatusEnum.VerificationFailed;
            }

            AddDomainEvent(new EmailNotVerifiedDomainEvent(Email, UserName, Id, LatestVerificationFailureMessage));

            return friendlyErrorResponse;
        }

        public bool AddPasswordHash(string password)
        {
            if (!TheUserIsInActiveState() || IsDeleted)
                throw new DomainException(
                    $"The user [{UserName} / {Email}] is either deactivated, deleted or has not yet verified his account, hence we are unable to add a password to it. Current registration status: {GetCurrentRegistrationStatus()}");
            PasswordHash = BC.HashPassword(password);
            return true;
        }

        // TODO: transform to event driven
        public bool CreatePasswordResetToken(string randomToken)
        {
            if (TheUserIsInActiveState() && !IsDeleted && TheUserHadBeenVerified())
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
                    $"The user [{UserName} / {Email}] is either deactivated, deleted or has not yet verified his account, hence we are unable to create a password reset token for it. Current registration status: {GetCurrentRegistrationStatus()}");
            }

            AddDomainEvent(new UserRequestedPasswordResetDomainEvent(Email, UserName, Id, randomToken));

            return true;
        }

        public bool AddVerificationToken(string verificationToken)
        {
            if (TheUserIsInActiveState() && !IsDeleted)
            {
                EmailVerificationToken = verificationToken;
                VerificationTokenExpirationDate =
                    DateTime.UtcNow.AddHours(ApplicationWideConstants.VERIFICATION_TOKEN_EXPIRES_IN_HOURS);
            }
            else
            {
                throw new DomainException(
                    $"The user [{UserName} / {Email}] is either deactivated, deleted or has not yet verified his account, hence we are unable to add a verification token to it. Current registration status: {GetCurrentRegistrationStatus()}");
            }

            return true;
        }

        public bool RemoveStaleRefreshTokens()
        {
            if (TheUserIsInActiveState() && TheUserHadBeenVerified() && !IsDeleted)
                _refreshTokens.RemoveAll(x =>
                    x.Created.AddDays(ApplicationWideConstants.REFREST_TOKEN_TTL_HOURS) <= DateTime.UtcNow);
            else
                throw new DomainException(
                    $"The user [{UserName} / {Email}] is either deactivated, deleted or has not yet verified his account, hence we are unable to remove stale refresh tokens from it. Current registration status: {GetCurrentRegistrationStatus()}");

            return true;
        }

        public bool AddRefreshToken(RefreshToken.RefreshToken refreshToken)
        {
            if (TheUserIsInActiveState() && TheUserHadBeenVerified() && !IsDeleted)
                _refreshTokens.Add(refreshToken);
            else
                throw new DomainException(
                    $"The user [{UserName} / {Email}] is either deactivated, deleted or has not yet verified his account, hence we are unable to add a refresh token to it. Current registration status: {GetCurrentRegistrationStatus()}");

            return true;
        }

        public List<RefreshToken.RefreshToken> GetRefreshTokens()
        {
            if (TheUserIsInActiveState() && TheUserHadBeenVerified() && !IsDeleted)
                return _refreshTokens.ToList();
            throw new DomainException(
                $"The user [{UserName} / {Email}] is either deactivated, deleted or has not yet verified his account, hence we are unable to fetch refresh tokens for it. Current registration status: {GetCurrentRegistrationStatus()}");
        }

        private bool UpdatePasswordThenRemoveResetToken(string newPassword)
        {
            if (TheUserIsInActiveState() && TheUserHadBeenVerified() && !IsDeleted)
            {
                if (VerificationTokenHasNotExpired())
                {
                    var hashedPassword = BC.HashPassword(newPassword);
                    PasswordHash = hashedPassword;
                    PasswordReset = DateTime.UtcNow;
                    ResetToken = null;
                    ResetTokenExpires = null;
                }
                else
                {
                    throw new DomainException(
                        $"The reset token for user [{UserName} / {Email}] is either non-defined or has expired, hence we are unable to update the password, please request a new reset password link. . Current registration status: {GetCurrentRegistrationStatus()}");
                }
            }
            else
            {
                throw new DomainException(
                    $"The user [{UserName} / {Email}] is either deactivated, deleted or has not yet verified his account, hence we are unable to fetch a token to it. Current registration status: {GetCurrentRegistrationStatus()}");
            }

            AddDomainEvent(new PasswordResetCompletedDomainEvent(Email, UserName, Id));

            return true;
        }

        private bool VerificationTokenHasNotExpired()
        {
            return !string.IsNullOrEmpty(ResetToken) && ResetTokenExpires <= DateTime.UtcNow;
        }

        public bool OwnsToken(string refreshToken)
        {
            if (TheUserIsInActiveState() && TheUserHadBeenVerified() && !IsDeleted)
                return _refreshTokens?.Find(x => x.Token == refreshToken) != null;
            throw new DomainException(
                $"The user [{UserName} / {Email}] is either deactivated, deleted or has not yet verified his account, hence we are unable to fetch a token to it. Current registration status: {GetCurrentRegistrationStatus()}");
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        public RegistrationStatusEnum GetCurrentRegistrationStatus()
        {
            return _status;
        }

        public void SetAccountActivationMailResent()
        {
            _accountActivationMailsSendAttempts += 1;
            var journalEntry =
                new AccountJournalEntry(DateTime.UtcNow +
                                        " => Account activation code re-sent to the users e-mail address.");
            journalEntry.AttachActingUser(null);
            journalEntry.AttachUser(this);
            _journalEntries.Add(journalEntry);
            _status = RegistrationStatusEnum.VerificationEmailResent;
        }

        #endregion Public methods
    }
}