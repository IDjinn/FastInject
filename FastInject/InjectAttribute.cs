using Microsoft.Extensions.DependencyInjection;

namespace FastInject;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class InjectAttribute : Attribute
{
    public InjectAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }

    public ServiceLifetime Lifetime { get; }
}