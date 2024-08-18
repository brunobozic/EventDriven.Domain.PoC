using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Serilog;

namespace IdentityService.Domain.DomainEntities.UserAggregate.RefreshTokenEntity;

public class RefreshToken : SimpleDomainEntityOfT<long>
{
    #region FK

    public Guid ApplicationUserId { get; private set; }

    #endregion FK

    #region Public Methods

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new NotImplementedException();
    }

    #endregion Public Methods

    #region Constructors

    private RefreshToken() { }

    private RefreshToken(string token, string createdByIp)
    {
        Token = token;
        Expires = DateTime.UtcNow.AddDays(7);
        Created = DateTime.UtcNow;
        CreatedByIp = createdByIp;
    }

    public static RefreshToken NewRefreshTokenDraft(string token, string createdByIp)
    {
        return new RefreshToken(token, createdByIp);
    }

    internal void SetRevoked(string ipAddress)
    {
        this.RevokedByIp= ipAddress;
        this.DateModified = DateTime.UtcNow;
    }

    #endregion Constructors

    #region Navigation Properties

    public virtual User ApplicationUser { get; private set; }

    public string CreatedByIp { get; private set; }
    public string RevokedByIp { get; private set; }

    #endregion Navigation Properties

    #region Public properties

    public string Token { get; private set; }
    public DateTime Expires { get; private set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime Created { get; private set; }
    public DateTime? Revoked { get; private set; }
    public string ReplacedByToken { get; private set; }
    public bool IsActive => Revoked == null && !IsExpired;

    #endregion Public properties
}
