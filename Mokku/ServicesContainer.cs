using System.Collections.Concurrent;

namespace Mokku;

internal static class ServicesContainer
{
    static ServicesContainer()
    {
        ServicesRegister.RegisterServices();
    }

    private static readonly ConcurrentDictionary<Type, ServiceDescriptor> _services = [];

    public static void RegisterSingleton<TInterface, TService>() where TService : TInterface
    {
        _services[typeof(TInterface)] = new ServiceDescriptor(typeof(TInterface), typeof(TService), ServiceLifetime.Singleton);
    }

    public static void RegisterSingleton<TService>(TService implemetation)
    {
        if (implemetation is null) throw new ArgumentException($"Can't register null value for {typeof(TService)}");
        _services[typeof(TService)] = new ServiceDescriptor(typeof(TService), implemetation);
    }

    public static void RegisterTransient<TInterface, TService>() where TService : TInterface
    {
        _services[typeof(TInterface)] = new ServiceDescriptor(typeof(TInterface), typeof(TService), ServiceLifetime.Transient);
    }

    public static void RegisterTransient<TService>()
    {
        _services[typeof(TService)] = new ServiceDescriptor(typeof(TService), typeof(TService), ServiceLifetime.Transient);
    }

    public static TService Resolve<TService>()
    {
        return (TService) Resolve(typeof(TService));
    }

    private static object Resolve(Type serviceType)
    {
        if (!_services.TryGetValue(serviceType, out var descriptor))
            throw new Exception($"There's no registered service for {serviceType.Name}");

        if (descriptor.ServiceLifetime == ServiceLifetime.Singleton)
        {
            if (descriptor.Instance is null)
            {
                descriptor.Instance = CreateInstance(descriptor.ImplementationType);
            }

            return descriptor.Instance!;
        }

        return CreateInstance(descriptor.ImplementationType);
    }

    private static object CreateInstance(Type instanceType)
    {
        var constuctor = instanceType.GetConstructors()[0];
        var parameters = constuctor.GetParameters();

        if (parameters.Length == 0)
        {
            return Activator.CreateInstance(instanceType)!;
        }

        var parameterInstances = new object[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            parameterInstances[i] = Resolve(parameters[i].ParameterType);
        }

        return Activator.CreateInstance(instanceType, parameterInstances)!;
    }
}

internal enum ServiceLifetime
{
    Singleton,
    Transient
}

internal class ServiceDescriptor
{
    public ServiceLifetime ServiceLifetime { get; }
    public Type ServiceType { get; }
    public Type ImplementationType { get; }
    public object? Instance { get; set; }

    public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime serviceLifetime)
    {
        ServiceType = serviceType;
        ImplementationType = implementationType;
        ServiceLifetime = serviceLifetime;
    }

    public ServiceDescriptor(Type serviceType, object implementation)
    {
        ServiceType = serviceType;
        ImplementationType = serviceType;
        ServiceLifetime = ServiceLifetime.Singleton;
        Instance = implementation;
    }
}