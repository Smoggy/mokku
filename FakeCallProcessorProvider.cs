using Mokku.Interfaces;

namespace Mokku;

class FakeCallProcessorProvider : IFakeCallProcessorProvider
{
    public IFakeCallProcessor Fetch(object proxy)
    {
        return new FakeCallProcessor();
    }
}
