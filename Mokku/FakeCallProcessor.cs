using Mokku.Extensions;
using Mokku.Interfaces;

namespace Mokku;

class FakeCallProcessor(List<IInterceptionRule> rules) : IFakeCallProcessor
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

        // TODO move to some objects
        fakeObjectCall.SetReturnValue(fakeObjectCall.MethodInfo.ReturnType.GetDefaultValue());
    }
}
