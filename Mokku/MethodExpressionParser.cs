using System.Linq.Expressions;
using System.Reflection;

namespace Mokku;

/// <summary>
/// Used to parse a method expression
/// </summary>
static class MethodExpressionParser
{
    /// <summary>
    /// Parses method or property expression
    /// </summary>
    /// <param name="expression"> expression of the target method</param>
    /// <returns>ParsedExpression object that incapulates information about configured method or property</returns>
    /// <exception cref="InvalidOperationException">thrown when provided expression is not for method or property</exception>
    public static ParsedExpression ParseExpression(LambdaExpression expression)
    {
        return expression.Body switch
        {
            MethodCallExpression methodExpression => ParseMethodCallExpression(methodExpression),
            MemberExpression memberExpression => ParsePropertyCallExpression(memberExpression),
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
