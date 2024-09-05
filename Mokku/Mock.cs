using Mokku.DynamicProxy;
using Mokku.Exceptions;
using Mokku.InterceptionRules;
using Mokku.Interfaces;
using Mokku.RuleConfigurations;
using System.Linq.Expressions;

namespace Mokku;

public class Mock<T> where T : class
{
    private readonly IRuleBuilder _ruleBuilder = ServicesContainer.Resolve<IRuleBuilder>();
    private readonly IProxyOptions proxyOptions;
    private readonly List<IInterceptionRule> rules = [];

    public Mock()
    {
        proxyOptions = ProxyOptions.Default;
    }

    public Mock(Action<IMockOptions<T>> optionsBuilder)
    {
        proxyOptions = new ProxyOptions();
        var mockOptions = new MockOptions<T>(proxyOptions);
        optionsBuilder.Invoke(mockOptions);
    }

    public Mock<T> WithCallTo(Expression<Action<T>> expression, Action<IVoidConfiguration> configurationBuilder)
    {
        var parsedExpression = CreateParsedExpression(expression);

        rules.Add(_ruleBuilder.BuildVoidMethodCallRule(parsedExpression, configurationBuilder));

        return this;
    }

    public Mock<T> WithCallTo<TMember>(Expression<Func<T, TMember>> expression, Action<IReturnValueConfiguration<TMember>> configurationBuilder)
    {
        var parsedExpression = CreateParsedExpression(expression);

        rules.Add(_ruleBuilder.BuildReturnMethodCallRule(parsedExpression, configurationBuilder));

        return this;
    }

    public Mock<T> WithPropertySetter<TMember>(Expression<Func<T, TMember>> expression, Action<IPropertySetterWithArgumentConstraintConfiguration<TMember>> configurationBuilder)
    {
        var parsedExpression = CreateParsedExpression(expression);

        rules.Add(_ruleBuilder.BuildPropertySetterCallRule(parsedExpression, configurationBuilder));

        return this;
    }

    public T Build()
    {
        return (T)Create(typeof(T));
    }

    private object Create(Type proxyType)
    {
        var result = CastleDynamicProxyCreator.GenerateProxyForType(proxyType, proxyOptions.AdditionalInterfaces, new FakeCallProcessorProvider(rules));

        if (result.IsSuccess) return result.ProxyObject!;

        throw new ConfigurationException();
    }

    private static ParsedExpression CreateParsedExpression(LambdaExpression expression)
    {
        var parsedExpression = MethodExpressionParser.ParseExpression(expression);
        var (success, failMessage) = MethodInterceptorValidator.CanBeInterceptedForObject(parsedExpression.Method, typeof(T));
        if (!success || parsedExpression == null) throw new ConfigurationException(failMessage);

        return parsedExpression;
    }
}
