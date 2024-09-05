using Mokku.InterceptionRules;

namespace Mokku.RuleConfigurations;

class ReturnValueConfigurationBuilder<TMember>(MethodCallRule rule) : IReturnValueConfiguration<TMember>, IAfterCallWithRefAndOutArgumentsConfiguration<IReturnValueConfiguration<TMember>>
{
    private readonly MethodCallRule rule = rule;

    public IAfterCallWithRefAndOutArgumentsConfiguration<IReturnValueConfiguration<TMember>> Returns(TMember value)
    {
        rule.SetApplyAction((proxyObj) => proxyObj.SetReturnValue(value));
        return this;
    }

    public IAfterCallWithRefAndOutArgumentsConfiguration<IReturnValueConfiguration<TMember>> Returns(Func<TMember> valueProvider)
    {
        rule.SetApplyAction((proxyObj) => proxyObj.SetReturnValue(valueProvider()));
        return this;
    }

    public IAfterCallConfiguration<IReturnValueConfiguration<TMember>> Throws(Func<Exception> exceptionFactory)
    {
        rule.SetApplyAction((_) => throw exceptionFactory());
        return this;
    }

    public IAfterCallConfiguration<IReturnValueConfiguration<TMember>> Throws<TException>() where TException : Exception, new()
    {
        rule.SetApplyAction((_) => throw new TException());
        return this;
    }

    public IAfterCallConfiguration<IReturnValueConfiguration<TMember>> DoesNothing()
    {
        rule.SetApplyAction(BaseInterceptionRule.DefaultApplyAction);
        return this;
    }

    public IAfterCallConfiguration<IReturnValueConfiguration<TMember>> SetsRefAndOutArguments(params object?[] values)
    {
        rule.SetRefAndOutFunc(() => values);
        return this;
    }

    public IReturnValueConfiguration<TMember> Invokes(Action callback)
    {
        rule.SetAdditionalAction(callback);
        return this;
    }
}
