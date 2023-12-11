using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
using TrackableEntities.Common.Core;
using BC = BCrypt.Net.BCrypt;

namespace IdentityService.Domain.DomainEntities.UserAggregate;

public class User : BasicDomainEntity<Guid>, IAuditTrail, IAggregateRoot
{
    private const int NumYearsDefaultActivity = 1;
    private readonly List<AccountJournalEntry> _journalEntries;
    private readonly List<RefreshToken> _refreshTokens;
    private readonly List<UserAddress> _userAddresses;
    private readonly List<UserRole> _userRoles;

    private IServiceProvider _serviceProvider;

    /// <summary>
    ///     For EFCore
    /// </summary>
    private User()
    {
        _refreshTokens = new List<RefreshToken>();
        _userRoles = new List<UserRole>();
        _journalEntries = new List<AccountJournalEntry>();
        _userAddresses = new List<UserAddress>();
        UserResourceId = Guid.NewGuid();
    }

    public DateTimeOffset? DateOfBirth { get; private init; }

    public Guid? DeactivatedById { get; }

    //[EnumDataType(typeof(RoleEnum))] public string BasicRole { get; private set; }
    public string Email { get; private init; }

    public string EmailVerificationToken { get; private set; }

    public string FirstName { get; private init; }

    public string FullName { get; private init; }

    public IReadOnlyCollection<AccountJournalEntry> JournalEntries => _journalEntries;

    public string LastName { get; private init; }

    public DateTime LastVerificationFailureDate { get; private set; }

    public string LatestVerificationFailureMessage { get; private set; }

    public string NormalizedEmail { get; private init; }

    public string NormalizedUserName { get; private init; }

    public string Oib { get; private init; }

    public string PasswordHash { get; private set; }

    public DateTime? PasswordReset { get; private set; }

    public string PasswordResetMsg { get; }

    public Guid? ReactivatedById { get; }

    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;

    public string ResetToken { get; private set; }

    public DateTime? ResetTokenExpires { get; private set; }

    public bool TwoFactorEnabled { get; private init; }

    public Guid? UndeletedById { get; }

    public IReadOnlyCollection<UserAddress> UserAddresses => _userAddresses;

    public string UserName { get; private init; }

    public Guid UserResourceId { get; }

    public IReadOnlyCollection<UserRole> UserRoles => _userRoles;

    public DateTime? VerificationTokenExpirationDate { get; private set; }

    public DateTime Verified { get; private set; }

    private int _accountActivationMailsSendAttempts { get; set; }

    private string _latestVerificationFailureMessage { get; set; }

    private DateTime _latestVerificationFailureTime { get; set; }

    private RegistrationStatusEnum _status { get; set; }

    public static User NewActiveWithPassword(
        Guid userId, string email, string userName, string firstName, string lastName,
        string oib, DateTimeOffset? dateOfBirth, DateTimeOffset activeFrom,
        DateTimeOffset activeTo, string password, User creator, string origin)
    {
        ValidateUserParameters(email, userName, firstName, lastName, oib, dateOfBirth, password);
        activeFrom = activeFrom == DateTimeOffset.MinValue ? DateTimeOffset.UtcNow : activeFrom;
        activeTo = activeTo == DateTimeOffset.MinValue
            ? DateTimeOffset.UtcNow.AddYears(NumYearsDefaultActivity)
            : activeTo;

        var user = CreateUserInstance(userId, email, userName, firstName, lastName, oib, dateOfBirth,
            RegistrationStatusEnum.WaitingForVerification, false);
        user.Activate(activeFrom, activeTo, creator);
        user.AddPasswordHash(password);
        user.AddVerificationToken(RandomStringHelper.RandomTokenString());

        user.AddDomainEvent(new UserCreatedDomainEvent(userId, email, userName, firstName, lastName, oib, dateOfBirth,
            DateTime.UtcNow, user.EmailVerificationToken, creator?.Id, user.UserResourceId, origin,
            EventTypeEnum.UserCreatedDomainEvent));
        return user;
    }

    public static User NewActiveWithPasswordAndEmailVerified(
        Guid userId, string email, string userName, string firstName, string lastName,
        string oib, DateTimeOffset? dateOfBirth, DateTimeOffset activeFrom,
        DateTimeOffset activeTo, string password, User activator, string origin, bool isSeed)
    {
        ValidateUserParameters(email, userName, firstName, lastName, oib, dateOfBirth, password);
        activeFrom = activeFrom == DateTimeOffset.MinValue ? DateTimeOffset.UtcNow : activeFrom;
        activeTo = activeTo == DateTimeOffset.MinValue
            ? DateTimeOffset.UtcNow.AddYears(NumYearsDefaultActivity)
            : activeTo;

        var user = CreateUserInstance(userId, email, userName, firstName, lastName, oib, dateOfBirth,
            RegistrationStatusEnum.Verified, isSeed);
        user.Activate(activeFrom, activeTo, activator);
        user.AddPasswordHash(password);
        user.AddVerificationToken(RandomStringHelper.RandomTokenString());
        user.SetEmailIsVerified();

        user.AddDomainEvent(new UserCreatedDomainEvent(userId, email, userName, firstName, lastName, oib, dateOfBirth,
            DateTime.UtcNow, user.EmailVerificationToken, activator?.Id, user.UserResourceId, origin,
            EventTypeEnum.UserCreatedDomainEvent));
        return user;
    }

    public static User NewDraft(
        Guid userId, string email, string userName, string firstName, string lastName,
        string password, string role, User creator, string origin)
    {
        ValidateUserParameters(email, userName, firstName, lastName, password);
        var user = CreateUserInstance(userId, email, userName, firstName, lastName, "", DateTimeOffset.MinValue,
            RegistrationStatusEnum.WaitingForVerification, false);
        user.AddPasswordHash(password);
        user.AddVerificationToken(RandomStringHelper.RandomTokenString());

        user.AddDomainEvent(new UserCreatedDomainEvent(userId, email, userName, firstName, lastName, "",
            DateTimeOffset.MinValue, DateTimeOffset.MinValue, "", creator.Id, user.UserResourceId, origin,
            EventTypeEnum.UserCreatedDomainEvent));
        return user;
    }

    public void AccountActivationMailNotSent(string eMessage)
    {
        using (var scope = LogContext.PushProperty("Method", nameof(AccountActivationMailNotSent)))
        {
            Log.Information("Attempting to send account activation mail for user {UserId}, {Email}", Id, Email);

            _accountActivationMailsSendAttempts += 1;
            LogJournalEntry("Unable to send account activation code sent to users e-mail address");
            _status = RegistrationStatusEnum.VerificationEmailSent;
            _latestVerificationFailureMessage = eMessage;
            _latestVerificationFailureTime = DateTime.UtcNow;

            Log.Warning("Failed to send account activation mail: {ErrorMessage}", eMessage);
        }
    }

    public bool Activate(DateTimeOffset from, DateTimeOffset to, User activatedBy)
    {
        using var scope = LogContext.PushProperty("Method", nameof(Activate));
        Log.Information("Activating user {UserId}", Id);

        try
        {
            GuardAgainstInactiveStatus();
            LogJournalEntry("User activated.");
            base.Activate(from, to, activatedBy);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error activating user");

            throw;
        }
    }

    public bool AddPasswordHash(string password)
    {
        using var scope = LogContext.PushProperty("Method", nameof(AddPasswordHash));
        Log.Information("Adding password hash for user {UserId}", Id);

        try
        {
            GuardAgainstInactiveOrDeletedStatus();
            LogJournalEntry("Password hash added.");
            PasswordHash = BC.HashPassword(password);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error adding password hash");

            throw;
        }
    }

    public bool AddRefreshToken(RefreshToken refreshToken)
    {
        using var scope = LogContext.PushProperty("Method", nameof(AddRefreshToken));
        Log.Information("Adding refresh token for user {UserId}", Id);

        try
        {
            GuardAgainstInactiveOrUnverifiedStatus();

            _refreshTokens.Add(refreshToken);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error adding refresh token");

            throw;
        }
    }

    public bool AddRole(Role role, User roleGiver)
    {
        using (LogContext.PushProperty("Method", nameof(AddRole)))

        {
            try
            {
                ValidateUserStateForRoleModification();
                ValidateRoleState(role);

                var newJoin = UserRole.NewActivatedDraft(this, role, roleGiver);
                _userRoles.Add(newJoin);
                UpdateModifiedDate();
                AddRoleAssignmentEvent(this, role, roleGiver);

                Log.Information("Role {RoleName} added to User {UserId}", role.Name, Id);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error adding role {RoleName} to User {UserId}", role.Name, Id);

                throw;
            }
        }
    }

    public bool AddVerificationToken(string verificationToken)
    {
        using var scope = LogContext.PushProperty("Method", nameof(AddVerificationToken));
        Log.Information("Adding verification token for user {UserId}", Id);

        try
        {
            GuardAgainstInactiveStatus();

            EmailVerificationToken = verificationToken;
            VerificationTokenExpirationDate =
                DateTime.UtcNow.AddHours(ApplicationWideConstants.VERIFICATION_TOKEN_EXPIRES_IN_HOURS);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error adding verification token");

            throw;
        }
    }

    public bool AssignAddress(Address address, AddressType addressType, User addressAssigner)
    {
        using var scope = LogContext.PushProperty("Method", nameof(AssignAddress));
        Log.Information("Assigning address to user {UserId}, {Email}", Id, Email);

        try
        {
            ValidateActiveAndVerifiedUser();
            address.AssignAddressType(addressType, addressAssigner);
            var userAddress = UserAddress.NewActivatedDraft(this, address, addressAssigner);

            _userAddresses.Add(userAddress);
            UpdateModifiedDate();
            AddAddressAssignmentEvent(address, addressType, addressAssigner);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error assigning address to user");

            throw;
        }
    }

    public bool CreatePasswordResetToken(string randomToken)
    {
        using var scope = LogContext.PushProperty("Method", nameof(CreatePasswordResetToken));
        Log.Information("Creating password reset token for user {UserId}", Id);

        try
        {
            GuardAgainstInactiveOrUnverifiedStatus();

            ResetToken = randomToken;
            ResetTokenExpires = DateTime.UtcNow.AddHours(8);
            LogJournalEntry("Reset password token created.");
            AddDomainEvent(new UserRequestedPasswordResetDomainEvent(Email, UserName, Id, randomToken));

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating password reset token");

            throw;
        }
    }

    public bool Deactivate(DateTimeOffset from, DateTimeOffset to, User deactivatedBy, string reason)
    {
        using var scope = LogContext.PushProperty("Method", nameof(Deactivate));
        Log.Information("Deactivating user {UserId} for reason: {Reason}", Id, reason);

        try
        {
            GuardAgainstInactiveOrDeletedStatus();

            Deactivate(deactivatedBy, reason);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deactivating user");

            throw;
        }
    }

    public RegistrationStatusEnum GetCurrentRegistrationStatus()
    {
        return _status;
    }

    public List<RefreshToken> GetRefreshTokens()
    {
        using var scope = LogContext.PushProperty("Method", nameof(GetRefreshTokens));
        Log.Information("Fetching refresh tokens for user {UserId}", Id);

        try
        {
            if (TheUserIsInActiveState() && TheUserHadBeenVerified() && !IsDeleted) return _refreshTokens.ToList();

            throw new DomainException(
                $"The user [{UserName} / {Email}] is either deactivated, deleted, or has not yet verified their account, hence we are unable to fetch refresh tokens for it. Current registration status: {GetCurrentRegistrationStatus()}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error fetching refresh tokens");

            throw;
        }
    }

    public List<Role> GetUserRoles()
    {
        return _userRoles.Where(userRole => userRole.Active).Select(role => role.Role).ToList();
    }

    // Update the comments for IsExpired method
    public virtual bool IsExpired(DateTimeOffset theDate)
    {
        return ActiveTo < theDate;
    }

    public bool OwnsToken(string refreshToken)
    {
        using var scope = LogContext.PushProperty("Method", nameof(OwnsToken));
        Log.Information("Checking if user {UserId} owns token {Token}", Id, refreshToken);

        try
        {
            if (TheUserIsInActiveState() && TheUserHadBeenVerified() && !IsDeleted)
                return _refreshTokens?.Find(x => x.Token == refreshToken) != null;

            throw new DomainException(
                $"The user [{UserName} / {Email}] is either deactivated, deleted, or has not yet verified their account, hence we are unable to check the token ownership. Current registration status: {GetCurrentRegistrationStatus()}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error checking token ownership");

            throw;
        }
    }

    public bool RemoveAddressFromUser(Address addressToRemove, AddressType addressType, User addressRemover)
    {
        using var scope = LogContext.PushProperty("Method", nameof(RemoveAddressFromUser));
        Log.Information("Removing address from user {UserId}, {Email}", Id, Email);

        try
        {
            ValidateActiveAndVerifiedUser();

            var userAddressIdsToDelete = _userAddresses
                .Where(userAddress => userAddress.Address.AddressIdGuid == addressToRemove.AddressIdGuid &&
                                      !userAddress.TheAddressHasBeenDeleted())
                .Select(uid => uid.Id)
                .ToList();

            _userAddresses.RemoveAll(userAddress => userAddressIdsToDelete.Contains(userAddress.Id));
            UpdateModifiedDate();
            AddAddressRemovalEvent(addressToRemove, addressType, addressRemover);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error removing address from user");

            throw;
        }
    }

    public bool RemoveRole(Role role, User removerUser)
    {
        using (LogContext.PushProperty("Method", nameof(RemoveRole)))

        {
            try
            {
                ValidateUserStateForRoleModification();
                ValidateRoleState(role);

                var linkTableEntry = _userRoles.SingleOrDefault(userRole =>
                    userRole.Active && userRole.Role.Name.Trim().ToUpper() == role.Name);

                if (linkTableEntry == null)
                    throw new AggregateException("Could not find an entry in the join table by the role name of: [ " +
                                                 role.Name + " ]");

                _userRoles.Remove(linkTableEntry);
                UpdateModifiedDate();
                AddRoleRemovalEvent(this, role, removerUser);

                Log.Information("Role {RoleName} removed from User {UserId}", role.Name, Id);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error removing role {RoleName} from User {UserId}", role.Name, Id);

                throw;
            }
        }
    }

    public bool RemoveStaleRefreshTokens()
    {
        using (var scope = LogContext.PushProperty("Method", nameof(RemoveStaleRefreshTokens)))
        {
            Log.Information("Removing stale refresh tokens for user {UserId}", Id);

            try
            {
                if (TheUserIsInActiveState() && TheUserHadBeenVerified() && !IsDeleted)
                {
                    _refreshTokens.RemoveAll(x =>
                        x.Created.AddDays(ApplicationWideConstants.REFREST_TOKEN_TTL_HOURS) <= DateTime.UtcNow);
                    return true;
                }

                throw new DomainException(
                    $"The user [{UserName} / {Email}] is either deactivated, deleted, or has not yet verified their account, hence we are unable to remove stale refresh tokens from it. Current registration status: {GetCurrentRegistrationStatus()}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error removing stale refresh tokens");

                throw;
            }
        }
    }

    public bool RevokeToken(string token, string ipAddress, User revokerUser)
    {
        using (var scope = LogContext.PushProperty("Method", nameof(RevokeToken)))
        {
            Log.Information("Revoking token for user {UserId}", Id);

            try
            {
                var refreshTokenDomain = _refreshTokens.SingleOrDefault(tok =>
                    string.Equals(tok.Token.Trim(), token.Trim(), StringComparison.CurrentCultureIgnoreCase));

                if (refreshTokenDomain != null)
                {
                    refreshTokenDomain.Revoked = DateTime.UtcNow;
                    refreshTokenDomain.RevokedByIp = ipAddress;

                    LogJournalEntry("Token revoked.");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error revoking token");

                throw;
            }
        }
    }

    public bool SendAlreadyRegisteredEmail(string origin)
    {
        using var scope = LogContext.PushProperty("Method", nameof(SendAlreadyRegisteredEmail));

        try
        {
            var message = !string.IsNullOrEmpty(origin)
                ? $@"<p>If you don't know your password please visit the <a href=""{origin}/applicationUser/forgot-password"">forgot password</a> page.</p>"
                : "<p>If you don't know your password you can reset it via the <code>/applicationUsers/forgot-password</code> api route.</p>";

            var emailSubject = "Sign-up Verification API - Email Already Registered";
            var completeEmailMessageBody = $@"<h4>Email Already Registered</h4>
                     <p>Your email <strong>{Email}</strong> is already registered.</p>
                     {message}";
            var emailFrom = "admin.poc@gmail.com";

            AddDomainEvent(new AccountAlreadyRegisteredMailReadiedDomainEvent(
                Email, UserName, Id, emailSubject, completeEmailMessageBody, emailFrom
            ));

            Log.Information("Processed already registered email for {Email}", Email);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error sending already registered email for {Email}", Email);

            throw;
        }
    }

    public void SetAccountActivationMailResent()
    {
        using (var scope = LogContext.PushProperty("Method", nameof(SetAccountActivationMailResent)))
        {
            LogJournalEntry("Account activation code re-sent.");
            _status = RegistrationStatusEnum.VerificationEmailResent;

            try
            {
                _accountActivationMailsSendAttempts += 1;

                Log.Information("Account activation mail resent for user {UserId}", Id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error setting account activation mail as resent");

                throw;
            }
        }
    }

    public string SetEmailIsVerified()
    {
        using var scope = LogContext.PushProperty("Method", nameof(SetEmailIsVerified));
        var friendlyErrorResponse = "OK";

        try
        {
            if (TheUserIsInActiveState() && !IsDeleted && VerificationTokenExpirationDate >= DateTime.UtcNow)
            {
                Verified = DateTime.UtcNow;
                EmailVerificationToken = null;
                VerificationTokenExpirationDate = null;
                _status = RegistrationStatusEnum.Verified;

                AddDomainEvent(new EmailVerifiedDomainEvent(Email, UserName, Id, Verified));

                Log.Information("Email verified for user {UserId}", Id);

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

            Log.Warning("Failed to verify email for user {UserId}. Reason: {Reason}", Id, friendlyErrorResponse);
            return friendlyErrorResponse;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error setting email as verified");

            throw;
        }
    }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new NotImplementedException();
    }

    public bool VerifyPasswordResetTokenThenResetPassword(string tokenReceivedByEmail, string newPassword)
    {
        using var scope = LogContext.PushProperty("Method", nameof(VerifyPasswordResetTokenThenResetPassword));

        try
        {
            if (string.IsNullOrEmpty(tokenReceivedByEmail))
                throw new DomainException(
                    $"The token received by email must not be null. Unable to confirm password reset for user [ {UserName} ], email [ {Email} ].");
            if (string.IsNullOrEmpty(newPassword))
                throw new DomainException(
                    $"The new password must not be null. Unable to confirm password reset for user [ {UserName} ], email [ {Email} ].");

            Log.Information("Verifying password reset token for user {UserId}", Id);

            return ResetToken.Trim() == tokenReceivedByEmail.Trim() && UpdatePasswordThenRemoveResetToken(newPassword);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error verifying password reset token and resetting password");

            throw;
        }
    }

    private static User CreateUserInstance(Guid userId, string email, string userName, string firstName,
        string lastName, string oib, DateTimeOffset? dateOfBirth, RegistrationStatusEnum status, bool isSeed)
    {
        return new User
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
    }

    private static void ValidateUserParameters(string email, string userName, string firstName, string lastName,
        string oib = null, DateTimeOffset? dateOfBirth = null, string password = null)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
        if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName));
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentNullException(nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentNullException(nameof(lastName));
        if (!string.IsNullOrWhiteSpace(oib) && string.IsNullOrWhiteSpace(oib))
            throw new ArgumentNullException(nameof(oib));
        if (dateOfBirth.HasValue && dateOfBirth >= DateTimeOffset.UtcNow)
            throw new ArgumentException("Date of birth cannot be in the future", nameof(dateOfBirth));
        if (!string.IsNullOrWhiteSpace(password) && string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));
    }

    private void AddAddressAssignmentEvent(Address address, AddressType addressType, User addressAssigner)
    {
        AddDomainEvent(new AddressAssignedToUserDomainEvent(
            Id,
            UserName,
            Email,
            address.Id,
            address.Line1,
            address.Active,
            address.AddressIdGuid,
            address.HouseNumber,
            address.HouseNumberSuffix,
            address.FlatNr,
            address.Town.Id,
            address.Town.Name,
            address.Town.DateCreated,
            address.Town.DateModified,
            address.Town.ZipCode,
            address.PostalCode,
            address.DateCreated,
            address.DateModified,
            addressType.Id,
            addressType.Name,
            addressType.Active,
            addressType.Description,
            addressAssigner.Id,
            addressAssigner.UserName,
            addressAssigner.Email,
            DateTimeOffset.UtcNow
        ));
    }

    private void AddAddressRemovalEvent(Address address, AddressType addressType, User addressRemover)
    {
        AddDomainEvent(new AddressRemovedFromUserDomainEvent(
            Id,
            UserName,
            Email,
            address.Id,
            address.Line1,
            address.Active,
            address.AddressIdGuid,
            address.HouseNumber,
            address.HouseNumberSuffix,
            address.FlatNr,
            address.Town.Id,
            address.Town.Name,
            address.Town.DateCreated,
            address.Town.DateModified,
            address.Town.ZipCode,
            address.PostalCode,
            address.DateCreated,
            address.DateModified,
            addressType.Id,
            addressType.Name,
            addressType.Active,
            addressType.Description,
            addressRemover.Id,
            addressRemover.UserName,
            addressRemover.Email,
            DateTimeOffset.UtcNow
        ));
    }

    private void AddRoleAssignmentEvent(User user, Role role, User roleGiver)
    {
        var roleAssignedEvent = new RoleAssignedToUserDomainEvent(
            user.Id,
            user.UserName,
            user.Email,
            role.Id, // Assuming you have an Id property for the Role entity
            role.Name,
            DateTimeOffset.UtcNow, // Use the current UTC time
            roleGiver.Id,
            roleGiver.Email,
            roleGiver.UserName,
            DateTimeOffset.UtcNow // Use the current UTC time
        );

        // Add the role assigned event to the domain events of the user
        user.AddDomainEvent(roleAssignedEvent);
    }

    private void AddRoleRemovalEvent(User user, Role role, User removerUser)
    {
        // Create a domain event representing role removal
        var dateRemoved = DateTimeOffset.UtcNow; // Use the current UTC time
        var roleRemovalEvent = new RoleRemovedFromUserDomainEvent(
            user.Id,
            user.UserName,
            user.Email,
            role.Id, // Assuming you have an Id property for the Role entity
            role.Name,
            removerUser.Id,
            removerUser.UserName,
            removerUser.Email,
            dateRemoved // Pass the correct DateRemoved value
        );

        // Add the role removal event to the domain events of the user
        user.AddDomainEvent(roleRemovalEvent);
    }

    private void GuardAgainstInactiveOrDeletedStatus()
    {
        if (!TheUserIsInActiveState() || IsDeleted)
            throw new DomainException(
                $"The user [{UserName} / {Email}] cannot perform this action. Current status: {GetCurrentRegistrationStatus()}");
    }

    private void GuardAgainstInactiveOrUnverifiedOrDeletedStatus()
    {
        if (!TheUserIsInActiveState() || !TheUserHadBeenVerified() || IsDeleted)
        {
            var errorMsg =
                $"The user [{UserName} / {Email}] is either deactivated, deleted or has not yet verified his account.";
            Log.Error(errorMsg + " Current registration status: {RegistrationStatus}", GetCurrentRegistrationStatus());
            throw new DomainException(errorMsg);
        }
    }

    private void GuardAgainstInactiveOrUnverifiedStatus()
    {
        if (!TheUserIsInActiveState() || !TheUserHadBeenVerified() || IsDeleted)
            throw new DomainException(
                $"The user [{UserName} / {Email}] cannot perform this action. Current status: {GetCurrentRegistrationStatus()}");
    }

    private void GuardAgainstInactiveStatus()
    {
        if (!TheUserIsInActiveState() || IsDeleted)
            throw new DomainException(
                $"The user [{UserName} / {Email}] cannot perform this action. Current status: {GetCurrentRegistrationStatus()}");
    }

    private void LogJournalEntry(string message)
    {
        var journalEntry = new AccountJournalEntry($"{DateTime.UtcNow} => {message}");

        journalEntry.JournalId = Guid.NewGuid();

        journalEntry.AttachActingUser(null);
        journalEntry.AttachUser(this);
        journalEntry.TrackingState = TrackingState.Added;

        //_journalEntries.Add(journalEntry);
    }

    private bool TheUserHadBeenVerified()
    {
        return Verified != DateTime.MinValue;
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

    private void UpdateModifiedDate()
    {
        DateModified = DateTime.UtcNow;
    }

    private bool UpdatePasswordThenRemoveResetToken(string newPassword)
    {
        using (LogContext.PushProperty("Method", nameof(UpdatePasswordThenRemoveResetToken)))

        {
            try
            {
                GuardAgainstInactiveOrUnverifiedOrDeletedStatus();

                if (VerificationTokenHasNotExpired())
                {
                    var hashedPassword = BC.HashPassword(newPassword);
                    PasswordHash = hashedPassword;
                    PasswordReset = DateTime.UtcNow;
                    ResetToken = null;
                    ResetTokenExpires = null;

                    Log.Information("Password updated for User {UserId}", Id);
                }
                else
                {
                    var errorMsg =
                        $"The reset token for user [{UserName} / {Email}] is either non-defined or has expired.";

                    Log.Error(errorMsg + " Current registration status: {RegistrationStatus}",
                        GetCurrentRegistrationStatus());
                    throw new DomainException(errorMsg + " Please request a new reset password link.");
                }

                AddDomainEvent(new PasswordResetCompletedDomainEvent(Email, UserName, Id));
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating password for User {UserId}", Id);

                throw;
            }
        }
    }

    private void ValidateActiveAndVerifiedUser()
    {
        if (!TheUserIsInActiveState() || !TheUserHadBeenVerified() || IsDeleted)
            throw new DomainException(
                $"Unable to perform operation on user [{UserName} / {Email}]. Current registration status: {GetCurrentRegistrationStatus()}");
    }

    private void ValidateRoleState(Role role)
    {
        if (role.IsExpired(DateTime.Now) || role.IsDeleted)
            throw new DomainException($"The role entity [ {role.Name} ] is either expired or deleted.");
    }

    private void ValidateUserStateForRoleModification()
    {
        if (!TheUserIsInActiveState() || !TheUserHadBeenVerified() || IsDeleted)
            throw new DomainException(
                $"The user [{UserName} / {Email}] is either deactivated, deleted or has not yet verified his account. Current registration status: {GetCurrentRegistrationStatus()}");
    }

    private bool VerificationTokenHasNotExpired()
    {
        return !string.IsNullOrEmpty(ResetToken) && ResetTokenExpires <= DateTime.UtcNow;
    }
}