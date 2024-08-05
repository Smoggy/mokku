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
}

public class TestClass;