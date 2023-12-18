using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Autofac.Core.Activators.Reflection;

namespace IdentityService.Api;

public class AllConstructorFinder : IConstructorFinder
{
    private static readonly ConcurrentDictionary<Type, ConstructorInfo[]> Cache =
        new();

    public ConstructorInfo[] FindConstructors(Type targetType)
    {
        var result = Cache.GetOrAdd(targetType,
            t => t.GetTypeInfo().DeclaredConstructors.ToArray());

        try
        {
            var temp = result.Length > 0 ? result : throw new NoConstructorsFoundException(targetType, this);

            return temp;
        }
        catch (Exception)
        {
            throw new NoConstructorsFoundException(targetType, this);
        }
    }
}