# FastInject

Easy and fast way to inejct your services into DI container of your application.

### How to use it

```csharp
services.InjectAllFromRootType(typeof(Program));
```

### Example

```csharp
public static class DependencyInjector {
	public static IServiceCollection AddPresentation(this IServiceCollection services) {
		// all services flagged with Inject attribute will be registered into DI container.
		services.InjectAllFromRootType(typeof(DependencyInjector));
		return services;
	}

}
```