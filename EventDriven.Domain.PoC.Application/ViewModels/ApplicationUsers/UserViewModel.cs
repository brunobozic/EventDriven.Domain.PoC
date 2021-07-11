using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationRoles;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Response;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public bool EmailConfirmed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateModified { get; set; }
        public bool IsVerified { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string PasswordResetMsg { get; set; }
        public IList<RoleViewModel> Roles { get; set; }
        public IList<RefreshTokenViewModel> RefreshTokens { get; set; }
        public IList<UserRoleViewModel> UserRoles { get; set; }

        [NotMapped] public bool IsDraft { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime? VerificationTokenExpirationDate { get; set; }
        public string VerificationFailureLatestMessage { get; set; }
        public string UndeleteReason { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public string DeactivateReason { get; set; }
        public string DeleteReason { get; set; }
        public DateTime LastVerificationFailureDate { get; set; }
        public string BasicRole { get; set; }
        public UserViewModel CreatedBy { get; set; }
        public UserViewModel DeletedBy { get; set; }
        public UserViewModel ActivatedBy { get; set; }

        #region EmailVerificationToken operation

        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime? PasswordReset { get; set; }
        public string VerificationToken { get; set; }
        public DateTime Verified { get; set; }

        #endregion EmailVerificationToken operations
    }
}