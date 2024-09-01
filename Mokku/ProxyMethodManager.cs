using Mokku.Extensions;
using System.Reflection;

namespace Mokku;

/// <summary>
/// Allows to get MethodInfo for a specific type, and also to validate if two methods are equals
/// Used to validate if method can be mocked and if some specific method call has configured rule
/// </summary>
internal static class ProxyMethodManager
{
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
    public static MethodInfo? FindMethodThatWillBeInvoked(Type type, MethodInfo method)
    {
        return type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault(x => x.HasSameBaseMethodAs(method));
    }
}
