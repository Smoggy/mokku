using System.Linq.Expressions;
using System.Reflection;

namespace Mokku;

class ParsedArgumentExpression(Expression expression, ParameterInfo parameterInfo)
{
    public Expression ArgumentExpression { get; } = expression;
    public ParameterInfo ParameterInfo { get; } = parameterInfo;
}
