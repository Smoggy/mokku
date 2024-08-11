using System.Collections;

namespace Mokku.ArgumentConstaints;

internal class AgregatedArgumentConsraint(List<IArgumentConstraint> constraints) : IArgumentConstraint
{
    private readonly List<IArgumentConstraint> _constraints = constraints;

    public bool IsValid(object? argument)
    {
        var arguments = (argument as IEnumerable)?.Cast<object>();

        if (arguments is null || arguments.Count() != _constraints.Count)
        {
            return false;
        }

        return _constraints.Zip(arguments, (c, a) => c.IsValid(a)).All(x => x);
    }
}
