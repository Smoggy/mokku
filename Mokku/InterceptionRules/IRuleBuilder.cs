using Mokku.RuleConfigurations;

namespace Mokku.InterceptionRules;

internal interface IRuleBuilder
{
    MethodCallRule BuildVoidMethodCallRule(ParsedExpression expression, Action<IVoidConfiguration> configurationBuilder);
    MethodCallRule BuildReturnMethodCallRule<TMember>(ParsedExpression expression, Action<IReturnValueConfiguration<TMember>> configurationBuilder);
    PropertySetterCallRule BuildPropertySetterCallRule<TMember>(ParsedExpression expression, Action<IPropertySetterWithArgumentConstraintConfiguration<TMember>> configurationBuilder);
}
