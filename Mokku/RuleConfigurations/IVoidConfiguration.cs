namespace Mokku.RuleConfigurations;

public interface IVoidConfiguration :
    IDoNothingConfiguration<IVoidConfiguration>,
    IThrowExceptionConfiguration<IVoidConfiguration>,
    IRefAndOutArgumentsConfiguration<IVoidConfiguration>,
    ICallbackConfiguration<IVoidConfiguration>,
    IRuleConfiguration;
