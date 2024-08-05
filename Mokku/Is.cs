using Mokku.ArgumentConstaints;

namespace Mokku;

public static class Is
{
    public static class A<T>
    {
        public static IArgumentConstraintsConfigurator<T> That => new DefaultArgumentConstraintsConfigurator<T>(ConstraintCatchService.SaveConstraintCallback);
    }

    public static class An<T> 
    {
        public static IArgumentConstraintsConfigurator<T> That => new DefaultArgumentConstraintsConfigurator<T>(ConstraintCatchService.SaveConstraintCallback);
    }

    public static T Any<T>()
    {
        new DefaultArgumentConstraintsConfigurator<T>(ConstraintCatchService.SaveConstraintCallback).Matches(_ => true);
        return default!;
    }
}
public static class ArgumentConstraintsExtensions
{
    public static T IsGreaterThan<T>(this IArgumentConstraintsConfigurator<T> configurator, T val) where T : IComparable<T>
    {
        configurator.Matches(x => x.CompareTo(val) > 0);
        return default!;
    }
}