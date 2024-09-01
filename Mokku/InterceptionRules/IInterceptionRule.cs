using Mokku.Interfaces;

namespace Mokku.InterceptionRules;

interface IInterceptionRule
{
    bool CanBeAppliedTo(IFakeObjectCall fakeObjectCall);
    void Apply(IFakeObjectCall fakeObjectCall);
}