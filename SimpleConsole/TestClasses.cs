using System.Security.Cryptography;

namespace SimpleConsole;

public class Foo
{
    public string StringMethod()
    {
        return "test";
    }

    public virtual string VirtualString(string str)
    {
        return str;
    }

    public virtual string Property { get; set; }
}

public class Bar(string Name, int Age, Foo Foo)
{
    public string Name { get; } = Name;
    public int Age { get; } = Age;
    public Foo Foo { get; } = Foo;

    public virtual string? Property { get; set; }
}

public class DerivedFoo : Foo
{
    public override string VirtualString(string str)
    {
        return $"derived - {str}";
    }
}

public interface GenericBase<T>
{
    public T Method(T val);
}

public class GenericDeviced<T> : GenericBase<T>
{
    public virtual T Method(T val) => val;
}



public interface ITest
{
    public void StringInput(string str);
    public bool BoolReturn();
    public bool BoolReturn(string t);
}