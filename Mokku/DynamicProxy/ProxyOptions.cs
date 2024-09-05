using Mokku.Interfaces;

namespace Mokku.DynamicProxy;

/// <summary>
/// Options that will be passed to proxy creator
/// </summary>
internal class ProxyOptions : IProxyOptions
{
    private readonly List<Type> _additionalInterfaces = [];

    public IReadOnlyList<Type> AdditionalInterfaces => _additionalInterfaces.AsReadOnly();

    public void AddInterfaceToImplement(Type interfaceType)
    {
        if (!interfaceType.IsInterface)
        {
            throw new ArgumentException("Type must be an interface");
        }

        _additionalInterfaces.Add(interfaceType);
    }

    public static IProxyOptions Default => new DefaultProxyOptions();

    private class DefaultProxyOptions : IProxyOptions
    {
        public IReadOnlyList<Type> AdditionalInterfaces => [];

        public void AddInterfaceToImplement(Type interfaceType)
        {
        }
    }
}
