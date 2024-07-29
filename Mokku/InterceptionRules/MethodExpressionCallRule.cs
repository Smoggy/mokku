using Mokku.Extensions;
using System.Reflection;

namespace Mokku.InterceptionRules;

internal class MethodExpressionCallRule(ParsedExpression expression) : IInterceptionRule
{
    private static readonly Action<IFakeObjectCall> DefaultApplyAction = call => call.SetReturnValue(call.MethodInfo.ReturnType.GetDefaultValue());

    private readonly ParsedExpression _expression = expression;

    public void Apply(IFakeObjectCall fakeObjectCall)
    {
        DefaultApplyAction.Invoke(fakeObjectCall);
    }

    public bool CanBeAppliedTo(IFakeObjectCall fakeObjectCall)
    {
        return MethodMatches(fakeObjectCall.FakeObject.GetType(), fakeObjectCall.MethodInfo, _expression.Method) && ArgumentsMatches();
    }

    private bool MethodMatches(Type proxyType, MethodInfo callMethod, MethodInfo ruleMethod)
    {
        if (callMethod == ruleMethod)
        {
            return true;
        }

        var invokedMethodByCall = FindMethodThatWillBeInvoked(proxyType, callMethod);
        var invokedMethodByRule = FindMethodThatWillBeInvoked(proxyType, ruleMethod);

        return invokedMethodByCall is not null && invokedMethodByRule is not null && invokedMethodByCall.Equals(invokedMethodByRule);
    }

    private bool ArgumentsMatches()
    {
        return true;
    }

    private MethodInfo? FindMethodThatWillBeInvoked(Type type, MethodInfo method)
    {
        var typeMethod = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(x => x.HasSameBaseMethodAs(method)).FirstOrDefault();

        return typeMethod;
    }
}
