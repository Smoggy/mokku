using Mokku.ArgumentConstaints;
using System.Reflection;

namespace Mokku.InterceptionRules;

internal class PropertySetterCallRule(ParsedExpression expression, List<IArgumentConstraint> argumentConstraints) : BaseInterceptionRule(expression, argumentConstraints)
{
    public void OverrideArgumentConstraint(IArgumentConstraint constraint)
    {
        _argumentConstraints.Clear();
        _argumentConstraints.Add(constraint);
    }

    public ParameterInfo GetParameterInfo() => _expression.Method.GetParameters().Last();
}
