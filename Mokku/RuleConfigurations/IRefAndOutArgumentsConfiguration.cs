namespace Mokku.RuleConfigurations;

public interface IRefAndOutArgumentsConfiguration<out IReturnType>
{
    IAfterCallConfiguration<IReturnType> SetsRefAndOutArguments(params object?[] values);
}
