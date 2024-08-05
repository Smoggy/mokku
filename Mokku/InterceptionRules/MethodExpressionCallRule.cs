using Mokku.ArgumentConstaints;
using Mokku.Extensions;
using Mokku.Interfaces;
using System.Reflection;

namespace Mokku.InterceptionRules;

internal class MethodExpressionCallRule(ParsedExpression expression, IArgumentConstraint[] argumentConstraints) : IInterceptionRule
{
    private static readonly Action<IFakeObjectCall> DefaultApplyAction = call => call.SetReturnValue(call.MethodInfo.ReturnType.GetDefaultValue());

    private readonly ParsedExpression _expression = expression;
    private Action<IFakeObjectCall> _applyAction = DefaultApplyAction;
    private readonly IArgumentConstraint[] _argumentConstraints = argumentConstraints;

    public void Apply(IFakeObjectCall fakeObjectCall)
    {
        _applyAction.Invoke(fakeObjectCall);
    }

    public bool CanBeAppliedTo(IFakeObjectCall fakeObjectCall)
    {
        return MethodMatches(fakeObjectCall.FakeObject.GetType(), fakeObjectCall.MethodInfo, _expression.Method) && ArgumentsMatches(fakeObjectCall.Arguments);
    }

    public void SetApplyAction(Action<IFakeObjectCall> applyAction)
    {
        _applyAction = applyAction;
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

    private bool ArgumentsMatches(object[] arguments)
    {
        if (arguments.Length != _argumentConstraints.Length)
        {
            throw new Exception("Wrong number of arguments and constaints");
        }

        for(var i = 0; i < arguments.Length; i ++)
        {
            if (!_argumentConstraints[i].IsValid(arguments[i]))
            {
                return false;
            }
        }

        return true;
    }

    private MethodInfo? FindMethodThatWillBeInvoked(Type type, MethodInfo method)
    {
        var typeMethod = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault(x => x.HasSameBaseMethodAs(method));

        return typeMethod;
    }
}
