using System.Linq.Expressions;
using System.Reflection;

namespace Mokku.ArgumentConstaints;

internal class ConstraintCatchService : IConstraintCatchService
{
    private static readonly Action<IArgumentConstraint> OnUnathorizedCatchAttemptAction = (_) => throw new InvalidOperationException();

    public static ThreadLocal<Action<IArgumentConstraint>> OnConstraintSaveAction = new(() => OnUnathorizedCatchAttemptAction);

    public static void SaveConstraintCallback(IArgumentConstraint constraint) => OnConstraintSaveAction.Value!.Invoke(constraint);

    public IArgumentConstraint TryCatchTheConstraintFromExpression(Expression expression)
    {
        List<IArgumentConstraint> catchedConstraints = [];
        try
        {
            if (!TryGetValueWithoutCompile(expression, out object? value))
            {
                OnConstraintSaveAction.Value = catchedConstraints.Add;

                value = Expression.Lambda(expression).Compile().DynamicInvoke();
            } else
            {
                return value is null ? new NullArgumentConstraint() : new ValueEqualityArgumentConstraint(value);
            }
        } finally
        {
            OnConstraintSaveAction.Value = OnUnathorizedCatchAttemptAction;
        }

        return catchedConstraints.Count == 1 ? catchedConstraints[0] : throw new Exception("Too many constraints");
    }

    private static bool TryGetValueWithoutCompile(Expression? expression, out object? value)
    {
        if (expression is null)
        {
            value = true;
            return true;
        }

        switch(expression.NodeType)
        {
            case ExpressionType.Constant:
                value = ((ConstantExpression)expression).Value;
                return true;
            case ExpressionType.MemberAccess:
                var memberExpression = (MemberExpression)expression;

                if (memberExpression?.Member is FieldInfo fieldInfo && TryGetValueWithoutCompile(memberExpression.Expression, out var fieldValue))
                {
                    value = fieldInfo.GetValue(fieldValue);
                    return true;
                }

                if (memberExpression?.Member is PropertyInfo propertyInfo && TryGetValueWithoutCompile(memberExpression.Expression, out var propertyValue))
                {
                    value = propertyInfo.GetValue(propertyValue, null);
                    return true;
                }
                value = null;
                return false;
            default:
                value = null;
                return false;
        }
    }
}
