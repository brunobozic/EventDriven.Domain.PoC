using System;

namespace SharedKernel.Extensions;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class SingletonServiceAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ScopedServiceAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class TransientServiceAttribute : Attribute
{
}