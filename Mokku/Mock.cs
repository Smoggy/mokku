using Mokku.ArgumentConstaints;
using Mokku.InterceptionRules;
using Mokku.Interfaces;
using System.Linq.Expressions;

namespace Mokku;

public class Mock<T> where T : class
{
    private readonly T fakeObject;
    private readonly IProxyOptions proxyOptions;
    private readonly List<IInterceptionRule> rules = [];

    public Mock()
    {
        proxyOptions = ProxyOptions.Default;
        fakeObject = (T)Create(typeof(T));
    }

    public Mock(Action<IMockOptions<T>> optionsBuilder)
    {
        proxyOptions = new ProxyOptions();
        var mockOptions = new MockOptions<T>(proxyOptions);
        optionsBuilder.Invoke(mockOptions);

        fakeObject = (T)Create(typeof(T));
    }

    public Mock<T> WithCallTo(Expression<Action<T>> methodCallExpression, Action<IVoidConfiguration> configurationBuilder)
    {
        var parsedExpression = MethodExpressionParser.ParseExpression(methodCallExpression);
        var constraints = CreateArgumentConstraints(parsedExpression.ArgumentsExpressions);
        rules.Add(new MethodExpressionCallRule(parsedExpression, constraints));

        return this;
    }

    public Mock<T> WithCallTo<TMember>(Expression<Func<T, TMember>> expression, Action<IReturnValueConfiguration<TMember>> configurationBuilder)
    {
        var parsedExpression = MethodExpressionParser.ParseExpression(expression);

        var constraints = CreateArgumentConstraints(parsedExpression.ArgumentsExpressions);
        var rule = new MethodExpressionCallRule(parsedExpression, constraints);

        var ruleBuilder = new ReturnValueConfigurationBuilder<TMember>(rule);
        configurationBuilder.Invoke(ruleBuilder);

        rules.Add(rule);

        return this;
    }

    public T Build()
    {
        return fakeObject;
    }

    private object Create(Type proxyType)
    {
        var result = CastleDynamicProxyCreator.GenerateProxyForType(proxyType, proxyOptions.AdditionalInterfaces, new FakeCallProcessorProvider(rules));

        if (result.IsSuccess) return result.ProxyObject!;

        throw new InvalidOperationException();
    }

    private static IArgumentConstraint[] CreateArgumentConstraints(ParsedArgumentExpression[] argumentsExpressions)
    {
        var argumentConstraintCreator = new ArgumentConstraintCreator(new ConstraintCatchService());

        return argumentsExpressions.Select(argumentConstraintCreator.CreateArgumentConstraintFromArgumentExpression).ToArray();
    }
}

class ReturnValueConfigurationBuilder<TMember>(MethodExpressionCallRule rule) : IReturnValueConfiguration<TMember>
{
    private readonly MethodExpressionCallRule rule = rule;

    public void Returns(TMember value)
    {
        rule.SetApplyAction((proxyObj) => proxyObj.SetReturnValue(value));
    }

    public void Returns(Func<TMember> valueProvider)
    {
        rule.SetApplyAction((proxyObj) => proxyObj.SetReturnValue(valueProvider()));
    }

    public void Throws(Func<Exception> exceptionFactory)
    {
        rule.SetApplyAction((_) => throw exceptionFactory());
    }

    public void Throws<TException>() where TException : Exception, new()
    {
        rule.SetApplyAction((_) => throw new TException());
    }
}

public interface IReturnValueConfiguration<TMember> : IThrowExceptionConfiguration
{
    void Returns(TMember value);
    void Returns(Func<TMember> valueProvider);
}

public interface IVoidConfiguration : IThrowExceptionConfiguration;

public interface IThrowExceptionConfiguration
{
    void Throws(Func<Exception> exceptionFactory);
    void Throws<TException>() where TException : Exception, new();
}

interface IInterceptionRule
{
    bool CanBeAppliedTo(IFakeObjectCall fakeObjectCall);
    void Apply(IFakeObjectCall fakeObjectCall);
}