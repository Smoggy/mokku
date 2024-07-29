using Castle.DynamicProxy;
using Mokku.InterceptionRules;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;

namespace Mokku;

public class Mock<T> where T : class
{
    private T fakeObject;
    private readonly List<IInterceptionRule> rules = [];

    public Mock()
    {
        fakeObject = (T)Create(typeof(T));
    }

    public Mock<T> WithCallTo(Expression<Action<T>> methodCallExpression, Action<IVoidConfiguration> configuration)
    {
        var expressionParser = new MethodExpressionParser();
        var parsedExpression = expressionParser.ParseExpression(methodCallExpression);

        rules.Add(new MethodExpressionCallRule(parsedExpression));

        return this;
    }

    public Mock<T> WithCallTo<TMember>(Expression<Func<T, TMember>> expression, Action<IReturnValueConfiguration<TMember>> configuration)
    {
        var expressionParser = new MethodExpressionParser();
        var parsedExpression = expressionParser.ParseExpression(expression);

        rules.Add(new MethodExpressionCallRule(parsedExpression));

        return this;
    }

    public T Build()
    {
        return fakeObject;
    }

    private object Create(Type proxyType)
    {
        var result = CastleDynamicProxyCreator.GenerateProxyForType(proxyType, Type.EmptyTypes, new FakeCallProcessorProvider(rules));

        if (result.IsSuccess) return result.ProxyObject!;

        throw new InvalidOperationException();
    }
}

class MethodExpressionParser
{
    public ParsedExpression ParseExpression(LambdaExpression expression)
    {
        return Parse(expression);
    }

    private ParsedExpression Parse(LambdaExpression expression)
    {
        return expression.Body switch
        {
            MethodCallExpression methodExpression => ParseMethodCallExpression(methodExpression),
            _ => throw new InvalidOperationException()
        };
    }

    private static ParsedExpression ParseMethodCallExpression(MethodCallExpression expression)
    {
        var argumentExpressions = new ParsedArgumentExpression[expression.Arguments.Count];
        var methodParameters = expression.Method.GetParameters();
        for(var i = 0; i < argumentExpressions.Length; i++)
        {
            argumentExpressions[i] = new ParsedArgumentExpression(expression.Arguments[i], methodParameters[i]);
        }

        return new ParsedExpression(expression.Method, expression.Object!, argumentExpressions);
    }

    private static ParsedExpression ParsePropertyCallExpression(MemberExpression expression)
    {
        var property = expression.Member as PropertyInfo;

        if (property is null)
        {
            throw new Exception("Not a property");
        }

        return new ParsedExpression(property.GetGetMethod(true)!, expression.Expression, Array.Empty<ParsedArgumentExpression>());
    }
}

class ParsedExpression(MethodInfo method, Expression? expression, ParsedArgumentExpression[] argumentExpressions)
{
    public MethodInfo Method { get; } = method;
    public Expression? Expression { get; } = expression;
    public ParsedArgumentExpression[] ArgumentsExpressions { get; } = argumentExpressions;
}

class ParsedArgumentExpression(Expression expression, ParameterInfo parameterInfo)
{
    public Expression ArgumentExpression { get; } = expression;
    public ParameterInfo ParameterInfo { get; } = parameterInfo;
}


public interface IReturnValueConfiguration<TMember> : IThrowExceptionConfiguration
{
    void Returns(TMember value);
}

public interface IVoidConfiguration : IThrowExceptionConfiguration
{

}

public interface IThrowExceptionConfiguration
{
    void Throws(Func<Exception> exceptionFactory);
    void Throws<TException>() where TException : Exception;

}

interface IInterceptionRule
{
    bool CanBeAppliedTo(IFakeObjectCall fakeObjectCall);
    void Apply(IFakeObjectCall fakeObjectCall);
}

class CastleInvocationAdapter : IFakeObjectCall
{
    private readonly IInvocation _invocation;

    public CastleInvocationAdapter(IInvocation invocation)
    {
        _invocation = invocation;
        Arguments = _invocation.Arguments;
    }

    public MethodInfo MethodInfo => _invocation.Method;

    public IEnumerable<object> Arguments { get; }

    public object FakeObject => _invocation.Proxy;

    public void CallBaseMethod()
    {
        _invocation.Proceed();
    }

    public void SetReturnValue(object? value)
    {
        _invocation.ReturnValue = value;
    }
}

interface IFakeObjectCall
{
    MethodInfo MethodInfo { get; }
    IEnumerable<object> Arguments { get; }
    object FakeObject { get; }
    void SetReturnValue(object? value);
    void CallBaseMethod();
}