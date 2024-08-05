using System.Linq.Expressions;
using System.Reflection;

namespace Mokku;

static class MethodExpressionParser
{
    public static ParsedExpression ParseExpression(LambdaExpression expression)
    {
        return Parse(expression);
    }

    private static ParsedExpression Parse(LambdaExpression expression)
    {
        return expression.Body switch
        {
            MethodCallExpression methodExpression => ParseMethodCallExpression(methodExpression),
            _ => throw new InvalidOperationException()
        };
    }

    private static ParsedExpression ParseMethodCallExpression(MethodCallExpression expression)
    {
        var argumentExpressions = new ParsedArgumentExpression[expression.Arguments.Count];
        var methodParameters = expression.Method.GetParameters();
        for (var i = 0; i < argumentExpressions.Length; i++)
        {
            argumentExpressions[i] = new ParsedArgumentExpression(expression.Arguments[i], methodParameters[i]);
        }

        return new ParsedExpression(expression.Method, expression.Object, argumentExpressions);
    }

    private static ParsedExpression ParsePropertyCallExpression(MemberExpression expression)
    {
        if (expression.Member is not PropertyInfo property)
        {
            throw new Exception("Not a property");
        }

        return new ParsedExpression(property.GetGetMethod(true)!, expression.Expression, []);
    }
}
