using System.Reflection;

namespace Mokku.Extensions
{
    internal static class MethodInfoExtensions
    {
        public static bool HasSameBaseMethodAs(this MethodInfo method, MethodInfo other)
        {
            return method.GetBaseDefinitionIncludingGeneric().IsSameMethodAs(other.GetBaseDefinitionIncludingGeneric());
        }

        private static MethodInfo GetBaseDefinitionIncludingGeneric(this MethodInfo method)
        {
            if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
            {
                return method.GetGenericMethodDefinition().GetBaseDefinition();
            }

            return method.GetBaseDefinition();
        }

        private static bool IsSameMethodAs(this MethodInfo method, MethodInfo other)
        {
            return method.DeclaringType == other.DeclaringType && method.MetadataToken == other.MetadataToken && method.Module == method.Module && method.GetGenericArguments().SequenceEqual(other.GetGenericArguments());
        }
    }
}
