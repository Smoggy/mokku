using Mokku.InterceptionRules;

namespace Mokku.RuleConfigurations;

internal class RuleConfigurationFactory : IRuleConfigurationFactory
{
    public IPropertySetterWithArgumentConstraintConfiguration<TMember> CreatePropertySetterConfiguration<TMember>(PropertySetterCallRule rule)
    {
        return new PropertySetterConfigurationBuilder<TMember>(rule);
    }

    public IReturnValueConfiguration<TMember> CreateReturnValueConfiguration<TMember>(MethodCallRule rule)
    {
        return new ReturnValueConfigurationBuilder<TMember>(rule);
    }

    public IVoidConfiguration CreateVoidConfiguration(MethodCallRule rule)
    {
        return new VoidConfigurationBuilder(rule);
    }
}
