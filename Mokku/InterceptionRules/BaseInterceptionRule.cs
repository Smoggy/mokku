using Mokku.ArgumentConstaints;
using Mokku.Extensions;
using Mokku.Interfaces;

namespace Mokku.InterceptionRules;

internal abstract class BaseInterceptionRule(ParsedExpression expression, List<IArgumentConstraint> argumentConstraints) : IInterceptionRule
{
    public static readonly Action<IFakeObjectCall> DefaultApplyAction = call => call.SetReturnValue(call.MethodInfo.ReturnType.GetDefaultValue());
    protected Action<IFakeObjectCall> _applyAction = DefaultApplyAction;
    protected readonly List<IArgumentConstraint> _argumentConstraints = argumentConstraints;
    protected readonly List<Action> _additionalActions = [];
    protected ParsedExpression _expression = expression;

    public virtual void Apply(IFakeObjectCall fakeObjectCall)
    {
        foreach (var action in _additionalActions)
        {
            action.Invoke();
        }

        _applyAction.Invoke(fakeObjectCall);
    }

    public bool CanBeAppliedTo(IFakeObjectCall fakeObjectCall)
    {
        return ProxyMethodManager.MethodMatches(fakeObjectCall.FakeObject.GetType(), fakeObjectCall.MethodInfo, _expression.Method) && ArgumentsMatches(fakeObjectCall.Arguments!);
    }

    public void SetApplyAction(Action<IFakeObjectCall> applyAction)
    {
        _applyAction = applyAction;
    }

    public void SetAdditionalAction(Action action)
    {
        _additionalActions.Add(action);
    }

    protected bool ArgumentsMatches(object[] arguments)
    {
        if (arguments.Length != _argumentConstraints.Count)
        {
            throw new Exception("Wrong number of arguments and constaints");
        }

        for (var i = 0; i < arguments.Length; i++)
        {
            if (!_argumentConstraints[i].IsValid(arguments[i]))
            {
                return false;
            }
        }

        return true;
    }
}
