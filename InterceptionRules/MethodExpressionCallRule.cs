using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mokku.InterceptionRules;

internal class MethodExpressionCallRule : IInterceptionRule
{
    private readonly ParsedExpression _expression;

    public void Apply(IFakeObjectCall fakeObjectCall)
    {
        throw new NotImplementedException();
    }

    public bool CanBeAppliedTo(IFakeObjectCall fakeObjectCall)
    {
        return MethodMatches(fakeObjectCall.FakeObject.GetType(), fakeObjectCall.MethodInfo, _expression.Method) && ArgumentsMatches();
    }

    private bool MethodMatches(Type proxyType, MethodInfo callMethod, MethodInfo ruleMethod)
    {
        if (callMethod == ruleMethod)
        {
            return true;
        }



        return true;
    }

    private bool ArgumentsMatches()
    {
        return true;
    }
}
