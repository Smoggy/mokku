namespace Mokku.ArgumentConstaints;

internal class DefaultArgumentConstraintsConfigurator<T>(Action<IArgumentConstraint> onConstraintCreatedCallback) : IArgumentConstraintsConfigurator<T>
{
    private readonly Action<IArgumentConstraint> onConstraintCreatedCallback = onConstraintCreatedCallback;

    public T Matches(Func<T, bool> predicate)
    {
        onConstraintCreatedCallback(new MatchArgumentConstraint<T>(predicate));
        return default!;
    }
}
