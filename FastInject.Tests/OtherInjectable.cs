using Microsoft.Extensions.DependencyInjection;

namespace FastInject.Tests;

[Inject(ServiceLifetime.Singleton)]
public class OtherInjectable : IOtherInjectable, IOtherInterface
{
}