using Mokku.Interfaces;

namespace Mokku;

class FakeCallProcessor : IFakeCallProcessor
{
    private readonly List<IInterceptionRule> allRules = [];

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

        if (bestSuitingRule is not null)
        {
            bestSuitingRule.Apply(fakeObjectCall);
        }
        var a = 1;
    }
}
