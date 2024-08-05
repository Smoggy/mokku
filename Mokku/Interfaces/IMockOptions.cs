namespace Mokku.Interfaces;

public interface IMockOptions<T> where T : class
{
    IMockOptions<T> ShouldImplement(Type type);
    IMockOptions<T> ShouldImplement<TInterface>();
}
