using System.Reflection;

namespace Mokku;
/// <summary>
/// Validates if a method can be intercepted
/// </summary>
internal static class MethodInterceptorValidator
{
    public static (bool result, string? failReason) CanBeInterceptedForObject(MethodInfo method, Type targetType)
    {
        var actualMethod = GetMethodForActualType(method, targetType);

        var failMessage = GetFailMessage(actualMethod);

        return (failMessage is null, failMessage);
    }

    private static string? GetFailMessage(MethodInfo method)
    {
        if (method.IsFinal || !method.IsVirtual)
        {
            return "Non-virtuals methods or properties can't be mocked.";
        }

        if (method.IsStatic)
        {
            return method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false).Length != 0
                ? "Extension methods can't be mocked"
                : "Static methods or properties can't be mocked";
        }

        if (Castle.DynamicProxy.ProxyUtil.IsAccessible(method, out var failMessage)) {
            return failMessage;
        }

        return null;
    }

    private static MethodInfo GetMethodForActualType(MethodInfo method, Type targetType)
        => ProxyMethodManager.FindMethodThatWillBeInvoked(targetType, method) ?? method;
}
