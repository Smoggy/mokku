using Castle.DynamicProxy;
using Mokku.Interfaces;
using System.Reflection;

namespace Mokku.DynamicProxy;

/// <summary>
/// Responsible for creation castle proxy
/// </summary>
internal static class CastleDynamicProxyCreator
{
    private static readonly ProxyGenerator proxyGenerator = new();
    private static readonly IProxyGenerationHook ProxyGenerationHook = new InterceptEverythingHook();

    public static ProxyCreationResult GenerateProxyForType(Type proxyType, IReadOnlyList<Type> additionalInterfaces, IFakeCallProcessorProvider fakeCallProcessorProvider)
    {
        var options = new ProxyGenerationOptions(ProxyGenerationHook);
        var arguments = Array.Empty<object>();
        object? proxy;
        try
        {
            if (proxyType.IsInterface)
            {
                Type[] a = [proxyType, .. additionalInterfaces];
                // we can create it with CreateClassProxy as well, but seems like this method is faster
                proxy = proxyGenerator.CreateInterfaceProxyWithoutTarget(proxyType, [proxyType, .. additionalInterfaces], options, new ProxyInterceptor(fakeCallProcessorProvider));
            }
            else
            {
                if (proxyType.IsSealed)
                {
                    return new("Sealed type can't be mocked");
                }

                proxy = proxyGenerator.CreateClassProxy(
                        proxyType,
                        [.. additionalInterfaces],
                        options,
                        arguments,
                        new ProxyInterceptor(fakeCallProcessorProvider));
            }
        }
        catch (Exception ex)
        {
            return new(ex.Message);
        }

        return new(proxyObject: proxy);
    }
}

// Custom Hook that intercepts every method
internal class InterceptEverythingHook : IProxyGenerationHook
{
    private static readonly int HashCode = typeof(InterceptEverythingHook).GetHashCode();

    public void MethodsInspected()
    {
    }

    public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
    {
    }

    public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
    {
        return true;
    }

    public override int GetHashCode()
    {
        return HashCode;
    }

    public override bool Equals(object? obj)
    {
        return obj is InterceptEverythingHook;
    }
}
