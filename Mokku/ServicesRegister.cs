using Mokku.ArgumentConstaints;
using Mokku.InterceptionRules;
using Mokku.RuleConfigurations;

namespace Mokku;

internal static class ServicesRegister
{
    static ServicesRegister()
    {
        RegisterServices();
    }

    public static void RegisterServices()
    {
        ServicesContainer.RegisterTransient<IConstraintCatchService, ConstraintCatchService>();
        ServicesContainer.RegisterTransient<ArgumentConstraintCreator>();
        ServicesContainer.RegisterTransient<IRuleConfigurationFactory, RuleConfigurationFactory>();
        ServicesContainer.RegisterTransient<IRuleBuilder, RuleBuilder>();
    }
}
