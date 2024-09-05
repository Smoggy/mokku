using Mokku.Interfaces;

namespace Mokku.InterceptionRules;

/// <summary>
/// Incapsulates logic to apply on a configured method call
/// </summary>
interface IInterceptionRule
{
    /// <summary>
    /// Determines if a rule can be applied to the intercepted proxy method call
    /// </summary>
    /// <param name="fakeObjectCall">proxy method call that needs to be checked</param>
    /// <returns>True if rule can be applied</returns>
    bool CanBeAppliedTo(IFakeObjectCall fakeObjectCall);

    /// <summary>
    /// Applies rule to the to the intercepted proxy method call
    /// </summary>
    /// <param name="fakeObjectCall">intercepted proxy method call</param>
    void Apply(IFakeObjectCall fakeObjectCall);
}