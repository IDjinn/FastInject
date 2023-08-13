using Microsoft.Extensions.DependencyInjection;

namespace FastInject.Tests;

public class InjectionTestings
{
    [Fact]
    public void test_injection_custom_interface()
    {
        var services = new ServiceCollection();
        services.InjectAllFromRootType(typeof(InjectionTestings), typeof(IOtherInterface));

        var provider = services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<IOtherInterface>());
    }

    [Fact]
    public void test_injection()
    {
        var services = new ServiceCollection();
        services.InjectAllFromRootType(typeof(InjectionTestings));

        var provider = services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<IOtherInjectable>());
    }
}