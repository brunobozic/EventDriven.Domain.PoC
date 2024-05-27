using IdentityService.Domain.DomainEntities.DomainExceptions;
using IdentityService.Domain.DomainEntities.UserAggregate.AccountJournal;
using IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate.AddressDomainEvents;
using IdentityService.Domain.DomainEntities.UserAggregate.RefreshTokenEntity;
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.EmailSending;
using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.PasswordReset;
using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.Verification;
using Serilog;
using Serilog.Context;
using SharedKernel.DomainContracts;
using SharedKernel.DomainCoreInterfaces;
using SharedKernel.DomainImplementations.BaseClasses;
using SharedKernel.Helpers.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TrackableEntities.Common.Core;
using BC = BCrypt.Net.BCrypt;

namespace IdentityService.Domain.DomainEntities.UserAggregate;

public class User : BasicDomainEntity<Guid>, IAuditTrail, IAggregateRoot
{
    private const int NumYearsDefaultActivity = 1;
    private readonly List<AccountJournalEntry> _journalEntries = new();
    private readonly List<RefreshToken> _refreshTokens = new();
    private readonly List<UserAddress> _userAddresses = new();
    private readonly List<UserRole> _userRoles = new();

    private User() => UserResourceId = Guid.NewGuid();

    #region Collections

    public IReadOnlyCollection<AccountJournalEntry> JournalEntries => _journalEntries.AsReadOnly();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();
    public IReadOnlyCollection<UserAddress> UserAddresses => _userAddresses.AsReadOnly();

    #endregion Collections

    #region Private props

    private int _accountActivationMailsSendAttempts { get; set; }
    private string _latestVerificationFailureMessage { get; set; } = string.Empty;
    private DateTimeOffset _latestVerificationFailureTime { get; set; }
    private RegistrationStatusEnum _status { get; set; }

    #endregion Private props

    #region Public props

    public DateTimeOffset? DateOfBirth { get; private init; }
    public string Email { get; private init; }
    public string EmailVerificationToken { get; private set; } = string.Empty;
    public string FirstName { get; private init; }
    public string FullName { get; private init; }
    public string LastName { get; private init; }
    public DateTimeOffset LastVerificationFailureDate { get; private set; }
    public string LatestVerificationFailureMessage { get; private set; } = string.Empty;
    public string NormalizedEmail { get; private init; }
    public string NormalizedUserName { get; private init; }
    public string Oib { get; private init; }
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTimeOffset? PasswordReset { get; private set; }
    public string PasswordResetMsg { get; } = string.Empty;
    public string ResetToken { get; private set; } = string.Empty;
    public DateTimeOffset? ResetTokenExpires { get; private set; }
    public bool TwoFactorEnabled { get; private init; }
    public string UserName { get; private init; }
    public Guid UserResourceId { get; }
    public DateTimeOffset? VerificationTokenExpirationDate { get; private set; }
    public DateTimeOffset Verified { get; private set; }
    public Guid? ReactivatedById { get; set; }
    public Guid? DeactivatedById { get; set; }
    public Guid? UndeletedById { get; set; }

    #endregion Public props

    #region Factory methods

    private static User CreateUserInstance(Guid userId, string email, string userName, string firstName, string lastName, string oib, DateTimeOffset? dateOfBirth, RegistrationStatusEnum status, bool isSeed) =>
        new()
        {
            Id = userId,
            Email = email.Trim(),
            UserName = userName,
            NormalizedEmail = email.Trim().ToUpper(),
            NormalizedUserName = userName.Trim().ToUpper(),
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            FullName = $"{lastName.Trim()} {firstName.Trim()}",
            TwoFactorEnabled = false,
            _status = status,
            DateCreated = DateTime.UtcNow,
            TrackingState = TrackingState.Added,
            Oib = oib,
            DateOfBirth = dateOfBirth,
            IsSeed = isSeed
        };

    public static User NewActiveWithPassword(Guid userId, string email, string userName, string firstName, string lastName, string oib, DateTimeOffset? dateOfBirth, DateTimeOffset activeFrom, DateTimeOffset activeTo, string password, User creator, string origin)
    {
        ValidateUserParameters(email, userName, firstName, lastName, oib, dateOfBirth, password);

        activeFrom = activeFrom == DateTimeOffset.MinValue ? DateTimeOffset.UtcNow : activeFrom;
        activeTo = activeTo == DateTimeOffset.MinValue ? DateTimeOffset.UtcNow.AddYears(NumYearsDefaultActivity) : activeTo;

        var user = CreateUserInstance(userId, email, userName, firstName, lastName, oib, dateOfBirth, RegistrationStatusEnum.WaitingForVerification, false);
        user.Activate(activeFrom, activeTo, creator);
        user.AddPasswordHash(password);
        user.AddVerificationToken(RandomStringHelper.RandomTokenString());
        user.AddDomainEvent(new UserCreatedDomainEvent(userId, email, userName, firstName, lastName, oib, dateOfBirth, DateTime.UtcNow, user.EmailVerificationToken, creator?.Id, user.UserResourceId, origin, EventTypeEnum.UserCreatedDomainEvent));

        return user;
    }

    public static User NewActiveWithPasswordAndEmailVerified(Guid userId, string email, string userName, string firstName, string lastName, string oib, DateTimeOffset? dateOfBirth, DateTimeOffset activeFrom, DateTimeOffset activeTo, string password, User activator, string origin, bool isSeed)
    {
        ValidateUserParameters(email, userName, firstName, lastName, oib, dateOfBirth, password);

        activeFrom = activeFrom == DateTimeOffset.MinValue ? DateTimeOffset.UtcNow : activeFrom;
        activeTo = activeTo == DateTimeOffset.MinValue ? DateTimeOffset.UtcNow.AddYears(NumYearsDefaultActivity) : activeTo;

        var user = CreateUserInstance(userId, email, userName, firstName, lastName, oib, dateOfBirth, RegistrationStatusEnum.Verified, isSeed);
        user.Activate(activeFrom, activeTo, activator);
        user.AddPasswordHash(password);
        user.AddVerificationToken(RandomStringHelper.RandomTokenString());
        user.SetEmailIsVerified();
        user.AddDomainEvent(new UserCreatedDomainEvent(userId, email, userName, firstName, lastName, oib, dateOfBirth, DateTime.UtcNow, user.EmailVerificationToken, activator?.Id, user.UserResourceId, origin, EventTypeEnum.UserCreatedDomainEvent));

        return user;
    }

    public static User NewDraft(Guid userId, string email, string userName, string firstName, string lastName, string password, string role, User creator, string origin)
    {
        ValidateUserParameters(email, userName, firstName, lastName, password);

        var user = CreateUserInstance(userId, email, userName, firstName, lastName, "", DateTimeOffset.MinValue, RegistrationStatusEnum.WaitingForVerification, false);
        user.AddPasswordHash(password);
        user.AddVerificationToken(RandomStringHelper.RandomTokenString());
        user.AddDomainEvent(new UserCreatedDomainEvent(userId, email, userName, firstName, lastName, "", DateTimeOffset.MinValue, DateTimeOffset.MinValue, "", creator.Id, user.UserResourceId, origin, EventTypeEnum.UserCreatedDomainEvent));

        return user;
    }

    #endregion Factory methods

    public void AccountActivationMailNotSent(string eMessage)
    {
        ExecuteWithLogging(nameof(AccountActivationMailNotSent), () =>
        {
            _accountActivationMailsSendAttempts++;
            _status = RegistrationStatusEnum.VerificationEmailSent;
            _latestVerificationFailureMessage = eMessage;
            _latestVerificationFailureTime = DateTimeOffset.UtcNow;
            LogJournalEntry("Unable to send account activation code that needed to be sent to users e-mail address");
            Log.Error("Failed to send account activation mail: [ {eMessage} ]", eMessage);
            return true;
        }, this);
    }

    public new bool Activate(DateTimeOffset from, DateTimeOffset to, User activatedBy) =>
        ExecuteWithLogging(nameof(Activate), () =>
        {
            GuardAgainstInactiveStatus();
            Activate(from, to, activatedBy);
            LogJournalEntry("User activated.");
            return true;
        }, this);

    public bool AddPasswordHash(string password) =>
        ExecuteWithLogging(nameof(AddPasswordHash), () =>
        {
            GuardAgainstInactiveOrDeletedStatus();
            PasswordHash = BC.HashPassword(password);
            LogJournalEntry("Password hash added.");
            return true;
        }, this);

    public bool AddRefreshToken(RefreshToken refreshToken) =>
        ExecuteWithLogging(nameof(AddRefreshToken), () =>
        {
            GuardAgainstInactiveOrUnverifiedStatus();
            _refreshTokens.Add(refreshToken);
            return true;
        }, this);

    public bool AddRole(Role role, User roleGiver) =>
        ExecuteWithLogging(nameof(AddRole), () =>
        {
            ValidateUserStateForRoleModification();
            ValidateRoleState(role);
            _userRoles.Add(UserRole.NewActivatedDraft(this, role, roleGiver));
            UpdateModifiedDate();
            AddRoleAssignmentEvent(this, role, roleGiver);
            LogJournalEntry($"Role [ {role.Name} ] added to User by [{roleGiver.FullName}]");
            return true;
        }, this);

    public bool AddVerificationToken(string verificationToken) =>
        ExecuteWithLogging(nameof(AddVerificationToken), () =>
        {
            GuardAgainstInactiveStatus();
            EmailVerificationToken = verificationToken;
            VerificationTokenExpirationDate = DateTime.UtcNow.AddHours(ApplicationWideConstants.VERIFICATION_TOKEN_EXPIRES_IN_HOURS);
            return true;
        }, this);

    public bool AssignAddress(Address address, AddressType addressType, User addressAssigner) =>
        ExecuteWithLogging(nameof(AssignAddress), () =>
        {
            ValidateActiveAndVerifiedUser();
            address.AssignAddressType(addressType, addressAssigner);
            ValidateAddressState(address);
            _userAddresses.Add(UserAddress.NewActivatedDraft(this, address, addressAssigner));
            UpdateModifiedDate();
            AddAddressAssignmentEvent(address, addressType, addressAssigner);
            LogJournalEntry($"Address [ {address.Line1} ] added to User by [ {addressAssigner?.FullName} ]");
            return true;
        }, this);

    public bool CreatePasswordResetToken(string randomToken) =>
        ExecuteWithLogging(nameof(CreatePasswordResetToken), () =>
        {
            GuardAgainstInactiveOrUnverifiedStatus();
            ResetToken = randomToken;
            ResetTokenExpires = DateTime.UtcNow.AddHours(8);
            LogJournalEntry("Reset password token created.");
            AddDomainEvent(new UserRequestedPasswordResetDomainEvent(Email, UserName, Id, randomToken));
            return true;
        }, this);

    public bool Deactivate(DateTimeOffset from, DateTimeOffset to, User deactivatedBy, string reason) =>
        ExecuteWithLogging(nameof(Deactivate), () =>
        {
            GuardAgainstInactiveOrDeletedStatus();
            Deactivate(deactivatedBy, reason);
            LogJournalEntry($"User deactivated by [ {deactivatedBy?.FullName} ]. Reason: [ {reason} ]");
            return true;
        }, this);

    public RegistrationStatusEnum GetCurrentRegistrationStatus() => _status;

    public List<RefreshToken> GetRefreshTokens() =>
        ExecuteWithLogging(nameof(GetRefreshTokens), () =>
        {
            if (IsActive() && IsVerified() && !IsDeleted)
                return _refreshTokens.ToList();
            throw new DomainException($"The user [ {UserName} / {Email} ] is either deactivated, deleted, or has not yet verified their account, hence we are unable to fetch refresh tokens. Current registration status: {GetCurrentRegistrationStatus()}");
        }, this);

    public List<Role> GetUserRoles() =>
        ExecuteWithLogging(nameof(GetUserRoles), () =>
        {
            ValidateActiveAndVerifiedUser();
            return _userRoles.Where(userRole => userRole.Active && !userRole.IsDeleted).Select(userRole => userRole.Role).ToList();
        }, this);

    public bool OwnsToken(string refreshToken) =>
        ExecuteWithLogging(nameof(OwnsToken), () =>
        {
            if (IsActive() && IsVerified() && !IsDeleted)
                return _refreshTokens.Any(x => x.Token == refreshToken);
            throw new DomainException($"The user [ {UserName} / {Email} ] is either deactivated, deleted, or has not yet verified their account, hence we are unable to check the token ownership. Current registration status: {GetCurrentRegistrationStatus()}");
        }, this);

    public bool RemoveAddressFromUser(Address addressToRemove, AddressType addressType, User addressRemover) =>
        ExecuteWithLogging(nameof(RemoveAddressFromUser), () =>
        {
            ValidateActiveAndVerifiedUser();
            var userAddressIdsToDelete = _userAddresses.Where(userAddress => userAddress.Address.AddressIdGuid == addressToRemove.AddressIdGuid && !userAddress.TheAddressHasBeenDeleted()).Select(uid => uid.Id).ToList();
            _userAddresses.RemoveAll(userAddress => userAddressIdsToDelete.Contains(userAddress.Id));
            UpdateModifiedDate();
            AddAddressRemovalEvent(addressToRemove, addressType, addressRemover);
            LogJournalEntry($"Address [ {addressToRemove.Line1} ] of type [ {addressType.Name} ] removed by [ {addressRemover?.FullName} ].");
            return true;
        }, this);

    public bool RemoveRole(Role role, User removerUser) =>
        ExecuteWithLogging(nameof(RemoveRole), () =>
        {
            ValidateUserStateForRoleModification();
            ValidateRoleState(role);
            var linkTableEntry = _userRoles.SingleOrDefault(userRole => userRole.Active && userRole.Role.Name.Trim().ToUpper() == role.Name);
            if (linkTableEntry == null)
                throw new AggregateException($"Could not find an entry in the join table by the role name of: {role.Name}");
            _userRoles.Remove(linkTableEntry);
            UpdateModifiedDate();
            AddRoleRemovalEvent(this, role, removerUser);
            LogJournalEntry($"Role [ {role.Name} ]  removed by [ {removerUser?.FullName} ].");
            return true;
        }, this);

    public bool RemoveStaleRefreshTokens() =>
        ExecuteWithLogging(nameof(RemoveStaleRefreshTokens), () =>
        {
            if (IsActive() && IsVerified() && !IsDeleted)
            {
                _refreshTokens.RemoveAll(x => x.Created.AddDays(ApplicationWideConstants.REFREST_TOKEN_TTL_HOURS) <= DateTime.UtcNow);
                return true;
            }
            throw new DomainException($"The user [{UserName} / {Email}] is either deactivated, deleted, or has not yet verified their account, hence we are unable to remove stale refresh tokens. Current registration status: {GetCurrentRegistrationStatus()}");
        }, this);

    public bool RevokeToken(string token, string ipAddress, User revokerUser) =>
        ExecuteWithLogging(nameof(RevokeToken), () =>
        {
            var refreshTokenDomain = _refreshTokens.SingleOrDefault(tok => tok.Token.Equals(token.Trim(), StringComparison.CurrentCultureIgnoreCase));
            if (refreshTokenDomain != null)
            {
                refreshTokenDomain.SetRevoked(ipAddress);
             
                LogJournalEntry($"Token revoked by [ {revokerUser?.FullName} ].");
                return true;
            }
            return false;
        }, this);

    public bool SendAlreadyRegisteredEmail(string origin) =>
        ExecuteWithLogging(nameof(SendAlreadyRegisteredEmail), () =>
        {
            var message = !string.IsNullOrEmpty(origin) ? $@"<p>If you don't know your password please visit the <a href=""{origin}/applicationUser/forgot-password"">forgot password</a> page.</p>" : "<p>If you don't know your password you can reset it via the <code>/applicationUsers/forgot-password</code> api route.</p>";
            var emailSubject = "Sign-up Verification API - Email Already Registered";
            var completeEmailMessageBody = $@"<h4>Email Already Registered</h4><p>Your email <strong>{Email}</strong> is already registered.</p>{message}";
            var emailFrom = "admin.poc@gmail.com";
            LogJournalEntry($"Processed already registered email for [ {UserName} ] with email [ {Email} ].");
            AddDomainEvent(new AccountAlreadyRegisteredMailReadiedDomainEvent(Email, UserName, Id, emailSubject, completeEmailMessageBody, emailFrom));
            Log.Information("Processed already registered email for {Email}", Email);
            return true;
        }, this);

    public void SetAccountActivationMailResent()
    {
        ExecuteWithLogging(nameof(SetAccountActivationMailResent), () =>
        {
            _accountActivationMailsSendAttempts++;
            _status = RegistrationStatusEnum.VerificationEmailResent;
            LogJournalEntry("Account activation code re-sent.");
            Log.Information("Account activation mail resent for user {UserId} to {Email}", Id, Email);
            return true;
        }, this);
    }

    public string SetEmailIsVerified() =>
        ExecuteWithLogging(nameof(SetEmailIsVerified), () =>
        {
            if (IsActive() && !IsDeleted && VerificationTokenHasNotExpired())
            {
                Verified = DateTime.UtcNow;
                EmailVerificationToken = null;
                VerificationTokenExpirationDate = null;
                _status = RegistrationStatusEnum.Verified;
                AddDomainEvent(new EmailVerifiedDomainEvent(Email, UserName, Id, Verified));
                Log.Information("Email verified for user {UserId}", Id);
                return "OK";
            }
            var friendlyErrorResponse = new List<string>();
            if (!IsActive())
            {
                friendlyErrorResponse.Add($"Unable to verify user account {UserName} / {Email}, the account is inactive.");
                LatestVerificationFailureMessage += "Account is inactive.";
            }
            if (IsDeleted)
            {
                friendlyErrorResponse.Add($"Unable to verify user account {UserName} / {Email}, the account is deleted.");
                LatestVerificationFailureMessage += "Account is deleted.";
            }
            if (VerificationTokenExpirationDate <= DateTime.UtcNow)
            {
                friendlyErrorResponse.Add($"Unable to verify user account {UserName} / {Email}, the account verification URL has expired.");
                LatestVerificationFailureMessage += "Verification URL expired.";
                LastVerificationFailureDate = DateTime.UtcNow;
                _status = RegistrationStatusEnum.VerificationFailed;
            }
            AddDomainEvent(new EmailNotVerifiedDomainEvent(Email, UserName, Id, LatestVerificationFailureMessage));
            Log.Warning("Failed to verify email for user {Id}. Reason: {friendlyErrorResponse}. Current registration status: {GetCurrentRegistrationStatus}", Id, string.Join(", ", friendlyErrorResponse), GetCurrentRegistrationStatus());
            return string.Join(", ", friendlyErrorResponse);
        }, this);

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => throw new NotImplementedException();

    public bool VerifyPasswordResetTokenThenResetPassword(string tokenReceivedByEmail, string newPassword) =>
        ExecuteWithLogging(nameof(VerifyPasswordResetTokenThenResetPassword), () =>
        {
            if (string.IsNullOrEmpty(tokenReceivedByEmail) || string.IsNullOrEmpty(newPassword))
                throw new DomainException($"The token or new password must not be null. Unable to confirm password reset for user {UserName} / {Email}. Current registration status: {GetCurrentRegistrationStatus()}");
            Log.Information("Verifying password reset token for user {Id}. Current registration status: {GetCurrentRegistrationStatus()}", Id, GetCurrentRegistrationStatus());
            return ResetToken.Trim() == tokenReceivedByEmail.Trim() && UpdatePasswordThenRemoveResetToken(newPassword);
        }, this);

    private void AddAddressAssignmentEvent(Address address, AddressType addressType, User addressAssigner) =>
        AddDomainEvent(new AddressAssignedToUserDomainEvent(
            Id, UserName, Email, address.Id, address.Line1, address.Active, address.AddressIdGuid,
            address.HouseNumber, address.HouseNumberSuffix, address.FlatNr, address.Town.Id, address.Town.Name,
            address.Town.DateCreated, address.Town.DateModified, address.Town.ZipCode, address.PostalCode,
            address.DateCreated, address.DateModified, addressType.Id, addressType.Name, addressType.Active,
            addressType.Description, addressAssigner.Id, addressAssigner.UserName, addressAssigner.Email,
            DateTimeOffset.UtcNow));

    private void AddAddressRemovalEvent(Address address, AddressType addressType, User addressRemover) =>
        AddDomainEvent(new AddressRemovedFromUserDomainEvent(
            Id, UserName, Email, address.Id, address.Line1, address.Active, address.AddressIdGuid,
            address.HouseNumber, address.HouseNumberSuffix, address.FlatNr, address.Town.Id, address.Town.Name,
            address.Town.DateCreated, address.Town.DateModified, address.Town.ZipCode, address.PostalCode,
            address.DateCreated, address.DateModified, addressType.Id, addressType.Name, addressType.Active,
            addressType.Description, addressRemover.Id, addressRemover.UserName, addressRemover.Email,
            DateTimeOffset.UtcNow));

    private void AddRoleAssignmentEvent(User user, Role role, User roleGiver) =>
        AddDomainEvent(new RoleAssignedToUserDomainEvent(
            user.Id, user.UserName, user.Email, role.Id, role.Name, DateTimeOffset.UtcNow, roleGiver.Id,
            roleGiver.Email, roleGiver.UserName, DateTimeOffset.UtcNow, EventTypeEnum.RoleAssignedToUser));

    private void AddRoleRemovalEvent(User user, Role role, User removerUser) =>
        AddDomainEvent(new RoleRemovedFromUserDomainEvent(
            user.Id, user.UserName, user.Email, role.Id, role.Name, removerUser.Id, removerUser.UserName,
            removerUser.Email, DateTimeOffset.UtcNow));

    private void LogJournalEntry(string message) =>
        _journalEntries.Add(new AccountJournalEntry($"{DateTime.UtcNow} => {message}")
        {
            JournalId = Guid.NewGuid(),
            TrackingState = TrackingState.Added
        }.AttachUser(this));

    private void UpdateModifiedDate() => DateModified = DateTime.UtcNow;

    private bool UpdatePasswordThenRemoveResetToken(string newPassword) =>
        ExecuteWithLogging(nameof(UpdatePasswordThenRemoveResetToken), () =>
        {
            GuardAgainstInactiveOrUnverifiedOrDeletedStatus();
            if (VerificationTokenHasNotExpired())
            {
                PasswordHash = BC.HashPassword(newPassword);
                PasswordReset = DateTime.UtcNow;
                ResetToken = null;
                ResetTokenExpires = null;
                Log.Information("Password updated for User [ {UserId} ],[ {UserName} / {Email} ]", Id, UserName, Email);
                AddDomainEvent(new PasswordResetCompletedDomainEvent(Email, UserName, Id));
                return true;
            }
            var errorMsg = $"The reset token for user [ {UserName} / {Email} ] is either non-defined or has expired.";
            Log.Error(errorMsg + $" Current registration status: {GetCurrentRegistrationStatus()}");
            throw new DomainException(errorMsg + " Please request a new reset password link.");
        }, this);

    private static T ExecuteWithLogging<T>(string methodName, Func<T> action, User user)
    {
        using (SerilogHelper.PushMethodSpecificProperties(user, methodName))
        {
            try
            {
                Log.Information("{MethodName} called for user {UserId}", methodName, user.Id);
                return action();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName} for user {UserId}", methodName, user.Id);
                throw;
            }
        }
    }

    #region Helper methods for business rules

    private static void ValidateUserParameters(string email, string userName, string firstName, string lastName, string oib = null, DateTimeOffset? dateOfBirth = null, string password = null)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
        if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName));
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentNullException(nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentNullException(nameof(lastName));
        if (!string.IsNullOrWhiteSpace(oib) && string.IsNullOrWhiteSpace(oib)) throw new ArgumentNullException(nameof(oib));
        if (dateOfBirth.HasValue && dateOfBirth >= DateTimeOffset.UtcNow) throw new ArgumentException("Date of birth cannot be in the future", nameof(dateOfBirth));
        if (!string.IsNullOrWhiteSpace(password) && string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));
    }

    private void GuardAgainstInactiveOrDeletedStatus()
    {
        if (!IsActive() || IsDeleted)
            throw new DomainException($"The user [ {UserName} / {Email} ] cannot perform this action. Current status: {GetCurrentRegistrationStatus()}");
    }

    private void GuardAgainstInactiveOrUnverifiedOrDeletedStatus()
    {
        if (!IsActive() || !IsVerified() || IsDeleted)
            throw new DomainException($"The user [ {UserName} / {Email} ] is either deactivated, deleted or has not yet verified his account. Current registration status: {GetCurrentRegistrationStatus()}");
    }

    private void GuardAgainstInactiveOrUnverifiedStatus()
    {
        if (!IsActive() || !IsVerified() || IsDeleted)
            throw new DomainException($"The user [ {UserName} / {Email} ] cannot perform this action. Current status: {GetCurrentRegistrationStatus()}");
    }

    private void GuardAgainstInactiveStatus()
    {
        if (!IsActive() || IsDeleted)
            throw new DomainException($"The user [ {UserName} / {Email} ] cannot perform this action. Current status: {GetCurrentRegistrationStatus()}");
    }

    private bool IsVerified() => Verified != DateTime.MinValue;

    private bool IsActive() => ActiveTo == null || ActiveTo >= DateTimeOffset.UtcNow;

    private bool VerificationTokenHasNotExpired() => !string.IsNullOrEmpty(ResetToken) && ResetTokenExpires <= DateTime.UtcNow;

    private void ValidateActiveAndVerifiedUser()
    {
        if (!IsActive() || !IsVerified() || IsDeleted)
            throw new DomainException($"Unable to perform operation on user [ {UserName} / {Email} ]. Current registration status: {GetCurrentRegistrationStatus()}");
    }

    private void ValidateRoleState(Role role)
    {
        if (role.IsExpired(DateTime.UtcNow) || role.IsDeleted)
            throw new DomainException($"The role entity [ {role.Name} ] is either expired or deleted. Current registration status: {GetCurrentRegistrationStatus()}");
    }

    private void ValidateAddressState(Address address)
    {
        if (!address.Active || address.IsDeleted)
            throw new DomainException($"The address entity [ {address.Line1} ] is either inactive or deleted.");
    }

    private void ValidateUserStateForRoleModification()
    {
        if (!IsActive() || !IsVerified() || IsDeleted)
            throw new DomainException($"The user [ {UserName} / {Email} ] is either deactivated, deleted or has not yet verified his account. Current registration status: {GetCurrentRegistrationStatus()}");
    }



    #endregion Helper methods for business rules
}
