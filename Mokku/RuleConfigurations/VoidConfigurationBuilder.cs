using Mokku.InterceptionRules;

namespace Mokku.RuleConfigurations;

internal class VoidConfigurationBuilder(MethodCallRule rule) : IVoidConfiguration, IAfterCallConfiguration<IVoidConfiguration>
{
    private readonly MethodCallRule rule = rule;

    public IAfterCallConfiguration<IVoidConfiguration> Throws(Func<Exception> exceptionFactory)
    {
        rule.SetApplyAction((_) => throw exceptionFactory());
        return this;
    }

    public IAfterCallConfiguration<IVoidConfiguration> Throws<TException>() where TException : Exception, new()
    {
        rule.SetApplyAction((_) => throw new TException());
        return this;
    }

    public IAfterCallConfiguration<IVoidConfiguration> DoesNothing()
    {
        rule.SetApplyAction(MethodCallRule.DefaultApplyAction);
        return this;
    }

    public IAfterCallConfiguration<IVoidConfiguration> SetsRefAndOutArguments(params object?[] values)
    {
        rule.SetRefAndOutFunc(() => values);
        return this;
    }

    public IVoidConfiguration Invokes(Action callback)
    {
        rule.SetAdditionalAction(callback);
        return this;
    }
}
