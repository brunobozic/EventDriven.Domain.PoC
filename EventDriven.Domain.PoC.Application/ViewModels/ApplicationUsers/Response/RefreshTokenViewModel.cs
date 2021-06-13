using System;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Response
{
    public class RefreshTokenViewModel
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired { get; set; }
        public string ReplacedByToken { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string Token { get; set; }
        public bool IsActive { get; set; }
    }
}