using System.Reflection;

namespace Mokku.Interfaces;

interface IFakeObjectCall
{
    MethodInfo MethodInfo { get; }
    object?[] Arguments { get; }
    object FakeObject { get; }
    void SetReturnValue(object? value);
    void CallBaseMethod();
}
