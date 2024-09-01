namespace Mokku.UnitTests;

public interface IFoo
{
    void Bar();
    void Bar(string str);
    void ThreeArgsMethod(string str, int number, object obj);
    string StringReturnMethod();
    int IntReturnWithArguments(string str, int number, DateTime date);
    object ObjectReturnMethod();
    int IntProperty { get; }
    string StringProperty { get; set; }
    int MethodWithParams(params string[] args);
    void MethodWithRefAndOut(string str, ref string refStr, int count, out string outStr);
}

public class TestBaseClass
{
    public void NonVirtualMethod(int _)
    {
    }
    public virtual void ActionMethod(int arg) { }
    public virtual string ReturnMethod(int arg1, string arg2)
    {
        return arg2;
    }
    public string? NonVirtualProperty { get; set; }
    public virtual string? VirtualProperty { get; set; }
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