namespace Mokku.ArgumentConstaints;

internal interface IArgumentConstraint
{
    bool IsValid(object? argument);
}
