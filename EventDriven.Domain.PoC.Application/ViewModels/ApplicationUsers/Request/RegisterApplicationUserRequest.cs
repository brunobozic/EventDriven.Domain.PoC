using System;
using System.ComponentModel.DataAnnotations;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Request
{
    public record RegisterUserRequest(
      string Email,
      string ConfirmPassword,
      DateTimeOffset? DateOfBirth,
      string FirstName,
      string LastName,
      string Password,
      string UserName,
      string Oib,
      [property: Range(typeof(bool), "true", "true")] bool AcceptTerms
  );
}