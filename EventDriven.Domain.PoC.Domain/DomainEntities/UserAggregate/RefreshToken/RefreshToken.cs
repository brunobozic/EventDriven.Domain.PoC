using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RefreshToken
{
    //[Owned]
    public class RefreshToken : SimpleDomainEntityOfT<long>
    {
        #region FK

        public Guid ApplicationUserId { get; set; }

        #endregion FK


        #region Public Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region ctor

        private RefreshToken()
        {
        }

        private RefreshToken(string token, string revokedByIp)
        {
            Token = token;
            Expires = DateTime.UtcNow.AddDays(7);
            Created = DateTime.UtcNow;
            RevokedByIp = revokedByIp;
        }

        public static RefreshToken NewRefreshTokenDraft(string token, string revokedByIp)
        {
            var newtoken = new RefreshToken(token, revokedByIp);
            return newtoken;
        }

        #endregion ctor

        #region Navigation Properties

        public virtual User ApplicationUser { get; set; }

        public string CreatedByIp { get; set; }
        public string RevokedByIp { get; set; }

        #endregion Navigation Properties

        #region Public properties

        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }

        public DateTime? Revoked { get; set; }

        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;

        #endregion Public properties
    }
}