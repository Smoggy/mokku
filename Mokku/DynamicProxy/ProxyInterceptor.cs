using Castle.DynamicProxy;
using Mokku.Interfaces;

namespace Mokku.DynamicProxy;

/// <summary>
/// Implementation of Dynamic proxy interceptor
/// </summary>
/// <param name="fakeCallProcessorProvider"></param>
class ProxyInterceptor(IFakeCallProcessorProvider fakeCallProcessorProvider) : IInterceptor
{
    private readonly IFakeCallProcessorProvider fakeCallProcessorProvider = fakeCallProcessorProvider;

    public void Intercept(IInvocation invocation)
    {
        fakeCallProcessorProvider.Fetch(invocation.Proxy).Process(new CastleInvocationAdapter(invocation));
    }
}
