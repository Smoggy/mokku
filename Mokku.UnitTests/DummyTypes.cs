namespace Mokku.UnitTests;

public interface IFoo
{
    void Bar();
    void Bar(string str);
    void ThreeArgsMethod(string str, int number, object obj);
    string StringReturnMethod();
    int IntReturnWithArguments(string str, int number, DateTime date);
    int IntProperty { get; }
    string StringProperty { get; set; }
    int MethodWithParams(params string[] args);
    void MethodWithRefAndOut(string str, ref string refStr, int count, out string outStr);
    Task<string> AsyncMethod();
}

public class TestBaseClass
{
#pragma warning disable CA1822 // Mark members as static
    public void NonVirtualMethod(int _)
#pragma warning restore CA1822 // Mark members as static
    {
    }
    public virtual void ActionMethod(int arg) { }
    public virtual string ReturnMethod(int arg1, string arg2)
    {
        return arg2;
    }
    public string? NonVirtualProperty { get; set; }
    public virtual string? VirtualProperty { get; set; }
    public virtual int VirtualPropertyWithoutSetter { get; }
    public virtual int MethodWithParams(params string[] args) { return 0; }
    public virtual void MethodWithRefAndOut(string str, ref string refStr, int count, out string outStr)
    {
        outStr = string.Empty;
    }
    public virtual async Task<string> AsyncMethod()
    {
        return await Task.FromResult(string.Empty);
    }
    public static void StaticMethod() { }
}

public class TestDerivedClass : TestBaseClass
{
    public sealed override void ActionMethod(int arg)
    {
        base.ActionMethod(arg);
    }
}

public sealed class SealedClass
{
    public void Bar() { }
}