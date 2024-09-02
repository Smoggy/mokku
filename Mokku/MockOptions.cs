using Mokku.Interfaces;

namespace Mokku;

/// <summary>
/// Represents additional configuration options for proxy creation
/// </summary>
/// <typeparam name="T">proxy type</typeparam>
/// <param name="proxyOptions">proxy options that will be passed to dynamic proxy upon fake option creation</param>
internal class MockOptions<T>(IProxyOptions proxyOptions) : IMockOptions<T> where T : class
{
    private readonly IProxyOptions proxyOptions = proxyOptions;

    /// <summary>
    /// Specifies which additional type proxy object should implement
    /// </summary>
    /// <param name="type">Additional type that proxy should implement</param>
    /// <returns>mock options instance</returns>
    public IMockOptions<T> ShouldImplement(Type type)
    {
        proxyOptions.AddInterfaceToImplement(type);

        return this;
    }

    /// <summary>
    /// Specifies which additional type proxy object should implement
    /// </summary>
    /// <typeparam name="TInterface">Additional type that proxy should implement</typeparam>
    /// <returns>mock options instance</returns>
    public IMockOptions<T> ShouldImplement<TInterface>()
    {
        proxyOptions.AddInterfaceToImplement(typeof(TInterface));

        return this;
    }
}
