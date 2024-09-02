using Mokku.ArgumentConstaints;
using Mokku.InterceptionRules;
using System.Linq.Expressions;

namespace Mokku.RuleConfigurations;

internal class PropertySetterConfigurationBuilder<TMember>(PropertySetterCallRule rule) : IPropertySetterWithArgumentConstraintConfiguration<TMember>, IAfterCallConfiguration<IPropertySetterConfiguration<TMember>>
{
    private readonly PropertySetterCallRule rule = rule;

    public IPropertySetterConfiguration<TMember> When(TMember value)
    {
        When(() => value);
        return this;
    }

    public IPropertySetterConfiguration<TMember> When(Expression<Func<TMember>> constraintExpression)
    {
        var argumentConstraintCreator = new ArgumentConstraintCreator(new ConstraintCatchService());
        var argumentExpression = new ParsedArgumentExpression(constraintExpression.Body, rule.GetParameterInfo());
        var constraint = argumentConstraintCreator.CreateArgumentConstraintFromArgumentExpression(argumentExpression);

        rule.OverrideArgumentConstraint(constraint);

        return this;
    }

    public IAfterCallConfiguration<IPropertySetterConfiguration<TMember>> DoesNothing()
    {
        rule.SetApplyAction(MethodExpressionCallRule.DefaultApplyAction);
        return this;
    }

    public IPropertySetterConfiguration<TMember> Invokes(Action callback)
    {
        rule.SetAdditionalAction(callback);
        return this;
    }

    public IAfterCallConfiguration<IPropertySetterConfiguration<TMember>> Throws(Func<Exception> exceptionFactory)
    {
        rule.SetApplyAction((_) => throw exceptionFactory());
        return this;
    }

    public IAfterCallConfiguration<IPropertySetterConfiguration<TMember>> Throws<TException>() where TException : Exception, new()
    {
        rule.SetApplyAction((_) => throw new TException());
        return this;
    }
}
