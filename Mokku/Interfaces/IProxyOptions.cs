
namespace Mokku.Interfaces;

internal interface IProxyOptions
{
    IReadOnlyList<Type> AdditionalInterfaces { get; }

    void AddInterfaceToImplement(Type interfaceType);
}
