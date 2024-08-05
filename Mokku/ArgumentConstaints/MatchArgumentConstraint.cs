using Mokku.Extensions;

namespace Mokku.ArgumentConstaints;

internal class MatchArgumentConstraint<T>(Func<T, bool> condition) : ITypedArgumentConstraint
{
    private readonly Func<T, bool> _condition = condition;

    public Type ArgumentType => typeof(T);

    public bool IsValid(object? argument)
    {
        if (argument == null && !typeof(T).IsNullable())
        {
            return false;
        }

        if (argument is not T)
        {
            return false;
        }

        try
        {
            return _condition.Invoke((T)argument);
        }
        catch
        {
            // TODO redefine proper exception type
            throw new Exception();
        }
    }
}
