using Mokku.ArgumentConstaints;
using Mokku.RuleConfigurations;

namespace Mokku.InterceptionRules;

internal class RuleBuilder(ArgumentConstraintCreator argumentConstraintCreator, IRuleConfigurationFactory ruleConfigurationFactory) : IRuleBuilder
{
    private readonly ArgumentConstraintCreator _argumentConstraintCreator = argumentConstraintCreator;
    private readonly IRuleConfigurationFactory _ruleConfigurationFactory = ruleConfigurationFactory;

    public MethodCallRule BuildVoidMethodCallRule(ParsedExpression expression, Action<IVoidConfiguration> configurationBuilder)
    {
        var constraints = CreateArgumentConstraints(expression.ArgumentsExpressions);
        var rule = new MethodCallRule(expression, constraints);
        var ruleBuilder = _ruleConfigurationFactory.CreateVoidConfiguration(rule);
        configurationBuilder.Invoke(ruleBuilder);

        return rule;
    }

    public MethodCallRule BuildReturnMethodCallRule<TMember>(ParsedExpression expression, Action<IReturnValueConfiguration<TMember>> configurationBuilder)
    {
        var constraints = CreateArgumentConstraints(expression.ArgumentsExpressions);
        var rule = new MethodCallRule(expression, constraints);
        var ruleBuilder = _ruleConfigurationFactory.CreateReturnValueConfiguration<TMember>(rule);
        configurationBuilder.Invoke(ruleBuilder);

        return rule;
    }

    public PropertySetterCallRule BuildPropertySetterCallRule<TMember>(ParsedExpression expression, Action<IPropertySetterWithArgumentConstraintConfiguration<TMember>> configurationBuilder)
    {
        var updatedParsedExpression = PropertySetterHelper.CreateSetterExpressionFromGetter<TMember>(expression);
        var constraints = CreateArgumentConstraints(updatedParsedExpression.ArgumentsExpressions);
        var rule = new PropertySetterCallRule(updatedParsedExpression, constraints);

        var ruleBuilder = _ruleConfigurationFactory.CreatePropertySetterConfiguration<TMember>(rule);
        configurationBuilder.Invoke(ruleBuilder);

        return rule;
    }

    private List<IArgumentConstraint> CreateArgumentConstraints(ParsedArgumentExpression[] argumentsExpressions) =>
        argumentsExpressions.Select(_argumentConstraintCreator.CreateArgumentConstraintFromArgumentExpression).ToList();
}
