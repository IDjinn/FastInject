using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace FastInject;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "SwitchStatementHandlesSomeKnownEnumValuesWithDefault")]
public static class DependencyInjector
{
    public static IServiceCollection InjectAllFromRootType(this IServiceCollection services, Type rootType)
    {
        ArgumentNullException.ThrowIfNull(rootType);

        var typesToInject = rootType.Assembly.GetTypes()
            .Where(type => type.GetCustomAttributes<InjectAttribute>().Any());
        foreach (var type in typesToInject)
        {
            if (type.IsInterface) continue;

            var @interface = type.GetInterfaces().FirstOrDefault(inter => inter.Name.Contains(type.Name)) ??
                             type.GetInterfaces().FirstOrDefault();
            if (@interface is null) continue;

            var injectAttribute = type.GetCustomAttributes<InjectAttribute>().FirstOrDefault();
            switch (injectAttribute?.Lifetime)
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