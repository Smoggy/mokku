using Castle.DynamicProxy;
using Mokku.Interfaces;
using System.Reflection;

namespace Mokku;

/// <summary>
/// Incapculates castle proxy invocation of a proxied method
/// </summary>
class CastleInvocationAdapter : IFakeObjectCall
{
    private readonly IInvocation _invocation;

    public CastleInvocationAdapter(IInvocation invocation)
    {
        _invocation = invocation;
        Arguments = _invocation.Arguments;
    }

    public MethodInfo MethodInfo => _invocation.Method;

    public object?[] Arguments { get; }

    public object FakeObject => _invocation.Proxy;

    public void CallBaseMethod()
    {
        _invocation.Proceed();
    }

    public void SetArgumentValue(int index, object? value)
    {
        _invocation.SetArgumentValue(index, value);
    }

    public void SetReturnValue(object? value)
    {
        _invocation.ReturnValue = value;
    }
}