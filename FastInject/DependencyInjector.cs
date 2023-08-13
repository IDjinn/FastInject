using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace FastInject;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "SwitchStatementHandlesSomeKnownEnumValuesWithDefault")]
public static class DependencyInjector
{
    public static IServiceCollection InjectAllFromRootType(
        this IServiceCollection services,
        Type rootType,
        params Type[] extraTypesToInject
    )
    {
        ArgumentNullException.ThrowIfNull(rootType);
        var typesToInject = rootType.Assembly.GetTypes()
            .Where(type => type.GetCustomAttributes<InjectAttribute>().Any() ||
                           type.GetInterfaces().Any(extraTypesToInject.Contains));

        foreach (var type in typesToInject)
        {
            var @interface = extraTypesToInject.Length > 0
                ? type.GetInterfaces().FirstOrDefault(extraTypesToInject.Contains)
                : type.GetInterfaces().FirstOrDefault(inter => inter.Name.Contains(type.Name));

            if (@interface is null) continue;
            var injectAttribute = type.GetCustomAttributes<InjectAttribute>().FirstOrDefault()?.Lifetime ??
                                  ServiceLifetime.Singleton;
            switch (injectAttribute)
            {
                case ServiceLifetime.Scoped:
                    services.AddScoped(@interface, type);
                    break;

                case ServiceLifetime.Singleton:
                    services.AddSingleton(@interface, type);
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient(@interface, type);
                    break;
            }
        }

        return services;
    }
}