using Castle.DynamicProxy;
using Mokku.Interfaces;

namespace Mokku;

class ProxyInterceptor(IFakeCallProcessorProvider fakeCallProcessorProvider) : IInterceptor
{
    private readonly IFakeCallProcessorProvider fakeCallProcessorProvider = fakeCallProcessorProvider;

    public void Intercept(IInvocation invocation)
    {
        fakeCallProcessorProvider.Fetch(invocation.Proxy).Process(new CastleInvocationAdapter(invocation));
    }
}
