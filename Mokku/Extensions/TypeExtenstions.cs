namespace Mokku.Extensions;

internal static class TypeExtenstions
{
    public static object? GetDefaultValue(this Type type)
    {
        return type.IsValueType && !type.Equals(typeof(void)) ? Activator.CreateInstance(type) : null;
    }
}
