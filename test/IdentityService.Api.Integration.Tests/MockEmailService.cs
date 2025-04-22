using IdentityService.Application.DomainServices.EmailServices;

namespace IdentityService.Tests.Integration.Fixtures
{
    internal class MockEmailService : IEmailService
    {
        public void Send(string to, string subject, string html, string from = null)
        {
            throw new NotImplementedException();
        }
    }
}