# FastInject

Easy and fast way to inejct your services into DI container of your application.

### How to use it

Every service you want to inject just needs to use the `Inject` attribute,
with [ServiceLifetime](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicelifetime?view=dotnet-plat-ext-7.0)
to insert into your DI container.

```csharp
[Inject(ServiceLifetime.Singleton)]
public class SomeServie : ISomeService {
	// ...
}
```

Now just configure it at once in your DI.

```csharp
services.InjectAllFromRootType(typeof(Program));
```

### Example

```csharp
[Inject(ServiceLifetime.Singleton)]
public class SomeServie : ISomeService {
	// ...
}

public static class DependencyInjector {
	public static IServiceCollection AddPresentation(this IServiceCollection services) {
		// All services flagged with the Inject attribute will be registered
		services.InjectAllFromRootType(typeof(DependencyInjector));
		// Now every service with Inject attribute in same namespace of DependencyInjector class will be inserted automatically into DI container.
		
		return services;
	}

}
```