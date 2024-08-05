using Mokku.ArgumentConstaints;
using System.Linq.Expressions;

namespace Mokku;

internal class ArgumentConstraintCreator(IConstraintCatchService service)
{
    private readonly IConstraintCatchService _catchService = service;
    public IArgumentConstraint CreateArgumentConstraintFromArgumentExpression(ParsedArgumentExpression expression)
    {
        if (IsParamArgumentsExpression(expression))
        {

        }

        var constraint = CreateArgumentConstraintFromExpression(expression);

        return constraint;
    }

    private IArgumentConstraint CreateArgumentConstraintFromExpression(ParsedArgumentExpression expression)
    {
        var constraint = _catchService.TryCatchTheConstraintFromExpression(expression.ArgumentExpression);

        if (constraint is ITypedArgumentConstraint typeConstraint)
        {
            var parameterType = expression.ParameterInfo.ParameterType;
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
        return argumentExpression.ArgumentExpression is NewArrayExpression && argumentExpression.ParameterInfo.IsDefined(typeof(NewArrayExpression), true);
    }
}
