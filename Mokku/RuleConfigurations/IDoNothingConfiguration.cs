namespace Mokku.RuleConfigurations;

public interface IDoNothingConfiguration<out IReturnType>
{
    IAfterCallConfiguration<IReturnType> DoesNothing();
}
