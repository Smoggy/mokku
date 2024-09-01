namespace Mokku.RuleConfigurations;

public interface IThrowExceptionConfiguration<out IReturnType>
{
    IAfterCallConfiguration<IReturnType> Throws(Func<Exception> exceptionFactory);
    IAfterCallConfiguration<IReturnType> Throws<TException>() where TException : Exception, new();
}
