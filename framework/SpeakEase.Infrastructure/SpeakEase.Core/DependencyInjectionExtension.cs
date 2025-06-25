using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SpeakEase.Infrastructure.SpeakEase.Core;

public interface IScopedDependency
{
}

public interface ISingletonDependency
{
}

public interface ITransientDependency
{
}

public static class DependencyInjectionExtension
{
    private static readonly Type[] InjectionTypes =
    [
        typeof(ISingletonDependency),
        typeof(IScopedDependency),
        typeof(ITransientDependency)
    ];

    public static IServiceCollection AddAssemblyDependencyInjection(this IServiceCollection services, Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            InjectionWithInterface(services, type);
        }

        return services;
    }

    /// <summary>
    /// 使用接口注入
    /// </summary>
    private static void InjectionWithInterface(IServiceCollection services, Type type)
    {
        //获取所有接口
        var interfaces = type.GetInterfaces();

        if (interfaces.Length <= 1)
        {
            return;
        }

        //判断接口是否为依赖注入接口
        var hasInjectionInterface = interfaces.Any(p => InjectionTypes.Any(x => x == p));

        if (!hasInjectionInterface)
        {
            return;
        }

        //获取具体生命周期接口
        var injectionInterface = interfaces.FirstOrDefault(p => InjectionTypes.Contains(p));

        Type interfaceImpl;

        var firstInterface = interfaces.FirstOrDefault();

        //获取实现的接口
        if (firstInterface != injectionInterface || firstInterface == null)
        {
            interfaceImpl = firstInterface;
        }
        else
        {
            interfaceImpl = interfaces[1];
        }
        
        if (interfaceImpl == null || services.Any(p => p.ServiceType == injectionInterface))
        {
            return;
        }

        //注入接口
        AddServiceWithInterface(services, interfaceImpl, type, injectionInterface);
    }

    private static void AddServiceWithInterface(IServiceCollection services, Type interfaces, Type interfaceImpl, Type injection)
    {
        if (injection == typeof(ISingletonDependency))
        {
            services.AddSingleton(interfaces, interfaceImpl);
            return;
        }

        if (injection == typeof(IScopedDependency))
        {
            services.AddScoped(interfaces, interfaceImpl);
            return;
        }

        if (injection == typeof(ITransientDependency))
        {
            services.AddTransient(interfaces, interfaceImpl);
        }
    }
}