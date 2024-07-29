using Mokku.Interfaces;
using System.Collections.Generic;

namespace Mokku;

class FakeCallProcessorProvider(List<IInterceptionRule> rules) : IFakeCallProcessorProvider
{
    public IFakeCallProcessor Fetch(object proxy)
    {
        return new FakeCallProcessor(rules);
    }
}
