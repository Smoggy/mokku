using System.Linq.Expressions;
using System.Reflection;

namespace Mokku;

/// <summary>
/// Stores the metadata information for the configured call
/// </summary>
/// <param name="method">MethodInfo of the configured method</param>
/// <param name="expression">Original expression</param>
/// <param name="argumentExpressions">List of the parsed arguments</param>
class ParsedExpression(MethodInfo method, Expression? expression, ParsedArgumentExpression[] argumentExpressions)
{
    public MethodInfo Method { get; } = method;
    public Expression? Expression { get; } = expression;
    public ParsedArgumentExpression[] ArgumentsExpressions { get; } = argumentExpressions;
}
