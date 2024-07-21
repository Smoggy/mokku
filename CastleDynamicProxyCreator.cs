using Castle.DynamicProxy;
using Mokku.Interfaces;

namespace Mokku;

internal static class CastleDynamicProxyCreator
{
    private static readonly ProxyGenerator proxyGenerator = new();

    public static ProxyCreationResult GenerateProxyForType(Type proxyType, Type[] additionalInterfaces, IFakeCallProcessorProvider fakeCallProcessorProvider)
    {
        var options = new ProxyGenerationOptions();
        var arguments = Array.Empty<object>();
        object? proxy;
        try
        {
            if (proxyType.IsInterface)
            {
                proxy = proxyGenerator.CreateInterfaceProxyWithoutTarget(proxyType, [proxyType, .. additionalInterfaces], options, new ProxyInterceptor(fakeCallProcessorProvider));
            }
            else
            {
                if (proxyType.IsSealed)
                {
                    return new("Sealed type can't be mocked");
                }

                proxy =  proxyGenerator.CreateClassProxy(
                        proxyType,
                        additionalInterfaces,
                        options,
                        arguments,
                        new ProxyInterceptor(fakeCallProcessorProvider));
            }
        } catch (Exception ex)
        {
            return new(ex.Message);
        }

        return new(proxyObject: proxy);
    }
}
