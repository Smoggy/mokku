namespace Mokku.ArgumentConstaints;

internal class NullArgumentConstraint : IArgumentConstraint
{
    public bool IsValid(object? argument)
    {
        return argument is null;
    }
}
