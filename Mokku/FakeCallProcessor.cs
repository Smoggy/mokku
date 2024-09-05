using Mokku.Extensions;
using Mokku.InterceptionRules;
using Mokku.Interfaces;

namespace Mokku;

/// <summary>
/// Processes logic for intercepted call
/// Tries to find most sutable rule or returns default value
/// </summary>
/// <param name="rules"></param>
internal class FakeCallProcessor(List<IInterceptionRule> rules) : IFakeCallProcessor
{
    private readonly List<IInterceptionRule> allRules = rules;

    public void Process(IFakeObjectCall fakeObjectCall)
    {
        IInterceptionRule? bestSuitingRule = null;
        lock (allRules)
        {
            foreach (var rule in allRules)
            {
                if (rule.CanBeAppliedTo(fakeObjectCall))
                {
                    bestSuitingRule = rule;
                    break;
                }
            }
        }

        if (bestSuitingRule != null)
        {
            bestSuitingRule.Apply(fakeObjectCall);
            return;
        }

        // we don't find any rule and need to set the default value
        fakeObjectCall.SetReturnValue(fakeObjectCall.MethodInfo.ReturnType.GetDefaultValue());
    }
}
