using Mokku.Interfaces;

namespace Mokku;

internal class MockOptions<T>(IProxyOptions proxyOptions) : IMockOptions<T> where T : class
{
    private readonly IProxyOptions proxyOptions = proxyOptions;

    public IMockOptions<T> ShouldImplement(Type type)
    {
        proxyOptions.AddInterfaceToImplement(type);

        return this;
    }

    public IMockOptions<T> ShouldImplement<TInterface>()
    {
        proxyOptions.AddInterfaceToImplement(typeof(TInterface));

        return this;
    }
}
