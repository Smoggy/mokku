using Mokku.ArgumentConstaints;

namespace Mokku;

/// <summary>
/// Static wrapper class which serves as an entry point for configuring method argument constraints.
/// </summary>
public static class Is
{
    /// <summary>
    /// Returns argument constraint configurator object that will allow to set method constraint
    /// </summary>
    /// <typeparam name="T">The type of an argument</typeparam>
    public static class A<T>
    {
        public static IArgumentConstraintsConfigurator<T> That => new DefaultArgumentConstraintsConfigurator<T>(ConstraintCatchService.SaveConstraintCallback);
    }

    /// <summary>
    /// Returns argument constraint configurator object that will allow to set method constraint
    /// Same as A, just to follow English grammar and use it if type name starts with a vowel
    /// </summary>
    /// <typeparam name="T">The type of an argument</typeparam>
    public static class An<T>
    {
        /// <summary>
        /// Returns argument constraint configurator object that will allow to set method constraint
        /// </summary>
        public static IArgumentConstraintsConfigurator<T> That => new DefaultArgumentConstraintsConfigurator<T>(ConstraintCatchService.SaveConstraintCallback);
    }

    /// <summary>
    /// Predefined constraint that allows any value for an argument
    /// </summary>
    /// <returns>default T value</returns>
    public static T Any<T>()
    {
        new DefaultArgumentConstraintsConfigurator<T>(ConstraintCatchService.SaveConstraintCallback).Matches(_ => true);
        // we don't care about actual return value, it's needed for the compiler so it can validate method expression in configuration
        return default!;
    }
}

/// <summary>
/// Extension methods that provide simplified api for configuring method arguments constraints
/// </summary>
public static class ArgumentConstraintsExtensions
{
    public static T IsGreaterThan<T>(this IArgumentConstraintsConfigurator<T> configurator, T val) where T : IComparable<T>
    {
        configurator.Matches(x => x.CompareTo(val) > 0);
        return default!;
    }

    public static T IsLesserThan<T>(this IArgumentConstraintsConfigurator<T> configurator, T val) where T : IComparable<T>
    {
        configurator.Matches(x => x.CompareTo(val) < 0);
        return default!;
    }

    public static string Contains(this IArgumentConstraintsConfigurator<string> configurator, string val, StringComparison? stringComparison = null)
    {
        configurator.Matches(x => stringComparison.HasValue ? x.Contains(val, stringComparison.Value) : x.IndexOf(val) > 0);
        return default!;
    }
}