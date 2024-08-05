namespace Mokku.ArgumentConstaints;

internal class ValueEqualityArgumentConstraint(object value) : IArgumentConstraint
{
    private readonly object _expectedValue = value;

    public bool IsValid(object? argument)
    {
        if (argument == null)
        {
            return false;
        }

        return argument.Equals(_expectedValue);
    }
}
