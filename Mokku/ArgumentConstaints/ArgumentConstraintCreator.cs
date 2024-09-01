using System.Linq.Expressions;

namespace Mokku.ArgumentConstaints;

internal class ArgumentConstraintCreator(IConstraintCatchService service)
{
    private readonly IConstraintCatchService _catchService = service;
    public IArgumentConstraint CreateArgumentConstraintFromArgumentExpression(ParsedArgumentExpression expression)
    {
        if (IsParamArgumentsExpression(expression))
        {
            return CreateParamsArgumentConstraintFromExpression((NewArrayExpression)expression.ArgumentExpression);
        }

        var constraint = CreateArgumentConstraintFromExpression(expression.ArgumentExpression, expression.ParameterInfo.ParameterType);

        return constraint;
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

        if (constraint is ITypedArgumentConstraint typeConstraint)
        {
            if (!parameterType.IsAssignableFrom(typeConstraint.ArgumentType))
            {
                // TODO create exception type
                throw new Exception();
            }
        }

        return constraint;
    }

    private static bool IsParamArgumentsExpression(ParsedArgumentExpression argumentExpression)
    {
        return argumentExpression.ArgumentExpression is NewArrayExpression && argumentExpression.ParameterInfo.IsDefined(typeof(ParamArrayAttribute), true);
    }
}
