namespace Mokku.RuleConfigurations;

public interface ICallbackConfiguration<out IReturnType>
{
    IReturnType Invokes(Action callback);
}
