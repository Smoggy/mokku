namespace Mokku.ArgumentConstaints;

public interface IArgumentConstraintsConfigurator<T>
{
    T Matches(Func<T, bool> predicate);
}
