using IdentityService.Domain.DomainEntities.UserAggregate;
using IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;

public static class SerilogHelper
{
    private static Dictionary<string, object> GetCommonProperties(User user) => new()
    {
        { "UserName", user.UserName },
        { "Email", user.Email },
        { "CurrentRegistrationStatus", user.GetCurrentRegistrationStatus() },
        { "Verified", user.Verified },
        { "IsActive", user.Active },
        { "IsDeleted", user.IsDeleted },
        { "Id", user.Id },
        { "UserResourceId", user.UserResourceId }
    }; 
    private static Dictionary<string, object> GetCommonProperties(Role role) => new()
    {
        { "IsActive", role.Active },
        { "RoleIdGuid", role.RoleIdGuid },
        { "Name", role.Name }, 
        { "Id", role.Id },
        { "IsDeleted", role.IsDeleted }

    }; 
    private static Dictionary<string, object> GetCommonProperties(UserRole userRole) => new()
    {
        { "IsActive", userRole.Active },
        { "Id", userRole.Id },
        { "IsDeleted", userRole.IsDeleted },
        { "Username", userRole.User?.UserName },
        { "UserEmail", userRole.User?.Email },
        { "UserResourceId", userRole.User?.UserResourceId }
    };

    public static IDisposable PushCommonProperties(User user) => PushProperties(GetCommonProperties(user));

    public static IDisposable PushProperties(Dictionary<string, object> properties)
    {
        IDisposable[] contexts = new IDisposable[properties.Count];
        int i = 0;

        foreach (var property in properties)
        {
            contexts[i++] = LogContext.PushProperty(property.Key, property.Value);
        }

        return new DisposeAction(() =>
        {
            foreach (var context in contexts)
            {
                context.Dispose();
            }
        });
    }

    public static IDisposable PushMethodSpecificProperties(User user, string methodName)
    {
        var properties = GetCommonProperties(user);
        properties.Add("Method", methodName);
        return PushProperties(properties);
    }
    public static IDisposable PushMethodSpecificProperties(Role role, string methodName)
    {
        var properties = GetCommonProperties(role);
        properties.Add("Method", methodName);
        return PushProperties(properties);
    }
    public static IDisposable PushMethodSpecificProperties(UserRole userrole, string methodName)
    {
        var properties = GetCommonProperties(userrole);
        properties.Add("Method", methodName);
        return PushProperties(properties);
    }

    public static IDisposable PushMethodSpecificProperties(Address address, string methodName)
    {
        var properties = GetCommonProperties(address);
        properties.Add("Method", methodName);
        return PushProperties(properties);
    }

    private static Dictionary<string, object> GetCommonProperties(Address address) => new()
    {
        { "Id", address.Id },
        { "IsActive", address.Active },
        { "IsDeleted", address.IsDeleted },
        { "AddressName", address.Name },
    };

    private class DisposeAction : IDisposable
    {
        private readonly Action _action;

        public DisposeAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action();
        }
    }
}
