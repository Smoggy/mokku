namespace Mokku.RuleConfigurations;

public interface IReturnValueConfiguration<TMember> :
    IDoNothingConfiguration<IReturnValueConfiguration<TMember>>,
    IThrowExceptionConfiguration<IReturnValueConfiguration<TMember>>,
    IRefAndOutArgumentsConfiguration<IReturnValueConfiguration<TMember>>,
    ICallbackConfiguration<IReturnValueConfiguration<TMember>>,
    IRuleConfiguration
{
    IAfterCallWithRefAndOutArgumentsConfiguration<IReturnValueConfiguration<TMember>> Returns(TMember value);
    IAfterCallWithRefAndOutArgumentsConfiguration<IReturnValueConfiguration<TMember>> Returns(Func<TMember> valueProvider);
}
