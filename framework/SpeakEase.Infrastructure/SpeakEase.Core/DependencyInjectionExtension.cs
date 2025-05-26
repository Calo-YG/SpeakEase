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
    public static Type[] InjectionTypes = new Type[]
    {
        typeof(ISingletonDependency),
        typeof(IScopedDependency),
        typeof(ITransientDependency)
    };

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
        var interfasces = type.GetInterfaces();

        if (interfasces.Length <= 1)
        {
            return;
        }

        //判断接口是否为依赖注入接口
        var hasInjectionInterface = interfasces.Any(p => InjectionTypes.Any(x => x == p));

        if (!hasInjectionInterface)
        {
            return;
        }

        //获取具体生命周期接口
        var injectionInterface = interfasces.FirstOrDefault(p => InjectionTypes.Contains(p));

        Type interfaceImplete = null;

        var firstInterface = interfasces[0];

        //获取实现的接口
        if (firstInterface != injectionInterface)
        {
            interfaceImplete = firstInterface;
        }
        else
        {
            interfaceImplete = interfasces[1];
        }

        ///判断是否注入容器中、
        if (interfaceImplete == null || services.Any(p => p.ServiceType == injectionInterface))
        {
            return;
        }

        //注入接口
        AddServieWithInterface(services, interfaceImplete, type, injectionInterface);
        ;
    }

    private static void AddServieWithInterface(IServiceCollection services, Type interfaces, Type implete, Type injection)
    {
        if (injection == typeof(ISingletonDependency))
        {
            services.AddSingleton(interfaces, implete);
            return;
        }

        if (injection == typeof(IScopedDependency))
        {
            services.AddScoped(interfaces, implete);
            return;
        }

        if (injection == typeof(ITransientDependency))
        {
            services.AddTransient(interfaces, implete);
        }
    }
}