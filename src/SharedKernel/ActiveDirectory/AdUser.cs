using System;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;

namespace SharedKernel.ActiveDirectory;

public class AdUser
{
    public DateTime? applicationUserExpirationDate { get; set; }
    public DateTime? applicationUserLockoutTime { get; set; }
    public int BadLogonCount { get; set; }
    public string Description { get; set; }
    public string DisplayName { get; set; }
    public string DistinguishedName { get; set; }
    public string Domain { get; set; }
    public string EmailAddress { get; set; }
    public string EmployeeId { get; set; }
    public bool? Enabled { get; set; }
    public string GivenName { get; set; }
    public Guid? Guid { get; set; }
    public string HomeDirectory { get; set; }
    public string HomeDrive { get; set; }
    public DateTime? LastBadPasswordAttempt { get; set; }
    public DateTime? LastLogon { get; set; }
    public DateTime? LastPasswordSet { get; set; }
    public string MiddleName { get; set; }
    public string Name { get; set; }
    public bool PasswordNeverExpires { get; set; }
    public bool PasswordNotRequired { get; set; }
    public string SamapplicationUserName { get; set; }
    public string ScriptPath { get; set; }
    public SecurityIdentifier Sid { get; set; }
    public string Surname { get; set; }
    public bool UserCannotChangePassword { get; set; }
    public string UserPrincipalName { get; set; }
    public string VoiceTelephoneNumber { get; set; }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    public static AdUser CastToAdUser(UserPrincipal user)
    {
        return new AdUser
        {
            applicationUserExpirationDate = user.AccountExpirationDate,
            applicationUserLockoutTime = user.AccountLockoutTime,
            BadLogonCount = user.BadLogonCount,
            Description = user.Description,
            DisplayName = user.DisplayName,
            DistinguishedName = user.DistinguishedName,
            EmailAddress = user.EmailAddress,
            EmployeeId = user.EmployeeId,
            Enabled = user.Enabled,
            GivenName = user.GivenName,
            Guid = user.Guid,
            HomeDirectory = user.HomeDirectory,
            HomeDrive = user.HomeDrive,
            LastBadPasswordAttempt = user.LastBadPasswordAttempt,
            LastLogon = user.LastLogon,
            LastPasswordSet = user.LastPasswordSet,
            MiddleName = user.MiddleName,
            Name = user.Name,
            PasswordNeverExpires = user.PasswordNeverExpires,
            PasswordNotRequired = user.PasswordNotRequired,
            SamapplicationUserName = user.SamAccountName,
            ScriptPath = user.ScriptPath,
            Sid = user.Sid,
            Surname = user.Surname,
            UserCannotChangePassword = user.UserCannotChangePassword,
            UserPrincipalName = user.UserPrincipalName,
            VoiceTelephoneNumber = user.VoiceTelephoneNumber
        };
    }

    public string GetDomainPrefix()
    {
        return DistinguishedName
            .Split(',')
            .FirstOrDefault(x => x.ToLower().Contains("dc"))
            .Split('=')
            .LastOrDefault()
            .ToUpper();
    }
}