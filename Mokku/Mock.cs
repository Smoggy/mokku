using Mokku.ArgumentConstaints;
using Mokku.Exceptions;
using Mokku.InterceptionRules;
using Mokku.Interfaces;
using Mokku.RuleConfigurations;
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

        var (success, failMessage) = MethodInterceptorValidator.CanBeInterceptedForObject(parsedExpression.Method, typeof(T));

        if (!success) throw new ConfigurationException(failMessage);

        var constraints = CreateArgumentConstraints(parsedExpression.ArgumentsExpressions);

        var rule = new MethodExpressionCallRule(parsedExpression, constraints);

        configurationBuilder.Invoke(new VoidConfigurationBuilder(rule));

        rules.Add(rule);

        return this;
    }

    public Mock<T> WithCallTo<TMember>(Expression<Func<T, TMember>> expression, Action<IReturnValueConfiguration<TMember>> configurationBuilder)
    {
        var parsedExpression = MethodExpressionParser.ParseExpression(expression);

        var (success, failMessage) = MethodInterceptorValidator.CanBeInterceptedForObject(parsedExpression.Method, typeof(T));

        if (!success) throw new ConfigurationException(failMessage);

        var constraints = CreateArgumentConstraints(parsedExpression.ArgumentsExpressions);
        var rule = new MethodExpressionCallRule(parsedExpression, constraints);

        var ruleBuilder = new ReturnValueConfigurationBuilder<TMember>(rule);
        configurationBuilder.Invoke(ruleBuilder);

        rules.Add(rule);
        return this;
    }

    public Mock<T> WithPropertySetter<TMember>(Expression<Func<T, TMember>> expression, Action<IPropertySetterWithArgumentConstraintConfiguration<TMember>> configurationBuilder)
    {
        var parsedExpression = MethodExpressionParser.ParseExpression(expression);
        var (success, failMessage) = MethodInterceptorValidator.CanBeInterceptedForObject(parsedExpression.Method, typeof(T));

        if (!success) throw new ConfigurationException(failMessage);

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

        throw new ConfigurationException();
    }

    private static IArgumentConstraint[] CreateArgumentConstraints(ParsedArgumentExpression[] argumentsExpressions)
    {
        var argumentConstraintCreator = new ArgumentConstraintCreator(new ConstraintCatchService());

        return argumentsExpressions.Select(argumentConstraintCreator.CreateArgumentConstraintFromArgumentExpression).ToArray();
    }
}
