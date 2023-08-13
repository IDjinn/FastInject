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
            var interfaces = new List<Type>();
            interfaces.AddRange(type.GetInterfaces().Where(inter => inter.Name.Contains(type.Name)));
            interfaces.AddRange(type.GetInterfaces().Where(extraTypesToInject.Contains));
            if (!interfaces.Any() && type.GetInterfaces().FirstOrDefault() is not null)
                interfaces.Add(type.GetInterfaces().FirstOrDefault()!);

            if (!interfaces.Any()) continue;
            var injectAttribute = type.GetCustomAttributes<InjectAttribute>().FirstOrDefault()?.Lifetime ??
                                  ServiceLifetime.Singleton;
            foreach (var @interface in interfaces)
            {
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
        }

        return services;
    }
}