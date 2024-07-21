namespace Mokku.Interfaces;

internal interface IFakeCallProcessorProvider
{
    IFakeCallProcessor Fetch(object proxy);
}
