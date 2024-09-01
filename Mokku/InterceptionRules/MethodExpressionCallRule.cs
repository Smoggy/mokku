using Mokku.ArgumentConstaints;
using Mokku.Extensions;
using Mokku.Interfaces;
using System.Reflection;

namespace Mokku.InterceptionRules;

internal class MethodExpressionCallRule(ParsedExpression expression, IArgumentConstraint[] argumentConstraints) : IInterceptionRule
{
    public static readonly Action<IFakeObjectCall> DefaultApplyAction = call => call.SetReturnValue(call.MethodInfo.ReturnType.GetDefaultValue());
    private static readonly Func<object?[]> DefaultRefAndOutArgumentsSetter = () => [];
    private Action<IFakeObjectCall> _applyAction = DefaultApplyAction;
    private readonly IArgumentConstraint[] _argumentConstraints = argumentConstraints;
    private readonly List<Action> _additionalActions = [];

    private Func<object?[]> refAndOutArgumentsSetter = DefaultRefAndOutArgumentsSetter;

    public ParsedExpression Expression { get; } = expression;

    public void Apply(IFakeObjectCall fakeObjectCall)
    {
        foreach(var action in _additionalActions)
        {
            action.Invoke();
        }

        _applyAction.Invoke(fakeObjectCall);

        SetRefAndOutArguments(fakeObjectCall);
    }

    public bool CanBeAppliedTo(IFakeObjectCall fakeObjectCall)
    {
        return MethodMatches(fakeObjectCall.FakeObject.GetType(), fakeObjectCall.MethodInfo, Expression.Method) && ArgumentsMatches(fakeObjectCall.Arguments!);
    }

    public void SetApplyAction(Action<IFakeObjectCall> applyAction)
    {
        _applyAction = applyAction;
    }

    public void SetRefAndOutFunc(Func<object?[]> valueFactory)
    {
        refAndOutArgumentsSetter = valueFactory;
    }

    public void SetAdditionalAction(Action action)
    {
        _additionalActions.Add(action);
    }

    private static bool MethodMatches(Type proxyType, MethodInfo callMethod, MethodInfo ruleMethod)
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

    private static MethodInfo? FindMethodThatWillBeInvoked(Type type, MethodInfo method)
    {
        return type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault(x => x.HasSameBaseMethodAs(method));
    }

    private void SetRefAndOutArguments(IFakeObjectCall fakeObjectCall)
    {
        if (refAndOutArgumentsSetter == DefaultRefAndOutArgumentsSetter)
            return;

        var values = refAndOutArgumentsSetter.Invoke();
        var indexes = GetIndexesOfRefAndOutArguments(fakeObjectCall.MethodInfo.GetParameters());
        if (values.Length != indexes.Count)
        {
            // TODO add proper exception type
            throw new ArgumentException("");
        }

        foreach(var pair in values.Zip(indexes, (v, i) => new {Value = v, Index = i}))
        {
            fakeObjectCall.SetArgumentValue(pair.Index, pair.Value);
        }
    }

    private static List<int> GetIndexesOfRefAndOutArguments(ParameterInfo[] parameters)
    {
        var result = new List<int>();

        for (var i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].ParameterType.IsByRef)
            {
                result.Add(i);
            }
        }

        return result;
    }
}
