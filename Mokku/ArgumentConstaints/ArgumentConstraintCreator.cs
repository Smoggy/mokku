using System.Linq.Expressions;

namespace Mokku.ArgumentConstaints;

/// <summary>
/// Responsible for creation of the argument constraints
/// </summary>
/// <param name="service">Helper service that allows to execute argument constraint expression</param>
internal class ArgumentConstraintCreator(IConstraintCatchService service)
{
    private readonly IConstraintCatchService _catchService = service;

    public IArgumentConstraint CreateArgumentConstraintFromArgumentExpression(ParsedArgumentExpression expression)
    {
        if (IsParamArgumentsExpression(expression))
        {
            return CreateParamsArgumentConstraintFromExpression((NewArrayExpression)expression.ArgumentExpression);
        }

        return CreateArgumentConstraintFromExpression(expression.ArgumentExpression, expression.ParameterInfo.ParameterType);
    }

    private AgregatedArgumentConsraint CreateParamsArgumentConstraintFromExpression(NewArrayExpression expression)
    {
        var constraints = new List<IArgumentConstraint>();

        foreach (var exp in expression.Expressions)
        {
            constraints.Add(CreateArgumentConstraintFromExpression(exp, exp.Type));
        }

        return new AgregatedArgumentConsraint(constraints);
    }

    private IArgumentConstraint CreateArgumentConstraintFromExpression(Expression expression, Type parameterType)
    {
        var constraint = _catchService.TryCatchTheConstraintFromExpression(expression);

        if (constraint is ITypedArgumentConstraint typeConstraint && !parameterType.IsAssignableFrom(typeConstraint.ArgumentType))
        {
            throw new ArgumentException("");
        }

        return constraint;
    }

    private static bool IsParamArgumentsExpression(ParsedArgumentExpression argumentExpression)
    {
        return argumentExpression.ArgumentExpression is NewArrayExpression && argumentExpression.ParameterInfo.IsDefined(typeof(ParamArrayAttribute), true);
    }
}
