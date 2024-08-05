using System.Linq.Expressions;
using System.Reflection;

namespace Mokku;

class ParsedExpression(MethodInfo method, Expression? expression, ParsedArgumentExpression[] argumentExpressions)
{
    public MethodInfo Method { get; } = method;
    public Expression? Expression { get; } = expression;
    public ParsedArgumentExpression[] ArgumentsExpressions { get; } = argumentExpressions;
}
