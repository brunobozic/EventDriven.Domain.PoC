using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Autofac.Core.Activators.Reflection;

namespace SharedKernel.Helpers;

public class AllConstructorFinderMine : IConstructorFinder
{
    private static readonly ConcurrentDictionary<Type, ConstructorInfo[]> Cache =
        new();

    public ConstructorInfo[] FindConstructors(Type targetType)
    {
        var result = Cache.GetOrAdd(targetType,
            t => t.GetTypeInfo().DeclaredConstructors.ToArray());

        try
        {
            var temp = result.Length > 0 ? result : throw new NoConstructorsFoundException(targetType);

            return temp;
        }
        catch (Exception)
        {
            throw new NoConstructorsFoundException(targetType);
        }
    }
}