using Mokku.Exceptions;
using System.Linq.Expressions;

namespace Mokku;

/// <summary>
/// Allows to generate setter methodInfo from property getter
/// </summary>
internal static class PropertySetterHelper
{
    public static ParsedExpression CreateSetterExpressionFromGetter<TValue>(ParsedExpression getterExpression)
    {
        // getters and setters have special method name signatures
        if (!getterExpression.Method.IsSpecialName || !getterExpression.Method.Name.StartsWith("get_"))
        {
            throw new ConfigurationException("Provided expression is not for property");
        }
        var propertyName = getterExpression.Method.Name[4..];

        var targetType = getterExpression.Method.DeclaringType;

        var getterPropertyInfo = targetType!.GetProperty(propertyName) ?? throw new ConfigurationException("Can't find property with this signature");
        var setterMethod = getterPropertyInfo.GetSetMethod( )?? throw new ConfigurationException("Can't find setter for this property");

        // by default we should allow any value for setter so we need to define this constraint explicitly 
        Expression<Func<TValue>> argumentExpression = () => Is.Any<TValue>();

        var newSetterValueExpression = new ParsedArgumentExpression(argumentExpression.Body, setterMethod.GetParameters().Last());

        return new ParsedExpression(setterMethod, null, [newSetterValueExpression]);
    }
}
