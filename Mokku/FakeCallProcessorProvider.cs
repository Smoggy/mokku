using Mokku.InterceptionRules;
using Mokku.Interfaces;

namespace Mokku;

class FakeCallProcessorProvider(List<IInterceptionRule> rules) : IFakeCallProcessorProvider
{
    public IFakeCallProcessor Fetch(object proxy)
    {
        return new FakeCallProcessor(rules);
    }
}
