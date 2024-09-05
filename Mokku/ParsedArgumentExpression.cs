using System.Linq.Expressions;
using System.Reflection;

namespace Mokku;

/// <summary>
/// Stores the argument metadata of a configured call
/// </summary>
/// <param name="expression">original exression</param>
/// <param name="parameterInfo">parameterInfo of an argument</param>
class ParsedArgumentExpression(Expression expression, ParameterInfo parameterInfo)
{
    public Expression ArgumentExpression { get; } = expression;
    public ParameterInfo ParameterInfo { get; } = parameterInfo;
}
