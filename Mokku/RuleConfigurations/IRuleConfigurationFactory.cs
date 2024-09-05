using Mokku.InterceptionRules;

namespace Mokku.RuleConfigurations;

internal interface IRuleConfigurationFactory
{
    public IVoidConfiguration CreateVoidConfiguration(MethodCallRule rule);
    public IReturnValueConfiguration<TMember> CreateReturnValueConfiguration<TMember>(MethodCallRule rule);
    public IPropertySetterWithArgumentConstraintConfiguration<TMember> CreatePropertySetterConfiguration<TMember>(PropertySetterCallRule rule);
}
