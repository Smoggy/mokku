using Mokku.Extensions;
using System.Reflection;

namespace Mokku;

/// <summary>
/// Allows to get MethodInfo for a specific type, and also to validate if two methods are equals
/// Used to validate if method can be mocked and if some specific method call has configured rule
/// </summary>
internal static class ProxyMethodManager
{
    /// <summary>
    /// Checks if a method that was intercepted by castle proxy is equal to a configured method
    /// </summary>
    /// <param name="proxyType">Type of a proxy object</param>
    /// <param name="callMethod">methodInfo that was intercepted by castle proxy</param>
    /// <param name="ruleMethod">methodInfo that was configured during proxy creation</param>
    /// <returns>Returns whether proxy method call and configured rule method are equals</returns>
    public static bool MethodMatches(Type proxyType, MethodInfo callMethod, MethodInfo ruleMethod)
    {
        if (callMethod == ruleMethod)
        {
            return true;
        }

        var invokedMethodByCall = FindMethodThatWillBeInvoked(proxyType, callMethod);
        var invokedMethodByRule = FindMethodThatWillBeInvoked(proxyType, ruleMethod);

        return invokedMethodByCall is not null && invokedMethodByRule is not null && invokedMethodByCall.Equals(invokedMethodByRule);
    }

    /// <summary>
    /// Tries to find actual methodInfo of a type based on it parent class method info
    /// </summary>
    /// <param name="type">Type of a proxy object</param>
    /// <param name="method">methodInfo of a base class</param>
    /// <returns>methodInfo of a target class or null if method is not found</returns>
    public static MethodInfo? FindMethodThatWillBeInvoked(Type type, MethodInfo method)
    {
        return type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault(x => x.HasSameBaseMethodAs(method));
    }
}
