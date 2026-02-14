using System.Reflection;
using CodeLab.Application.Contracts.Fallback.Attributes;
using CodeLab.Application.Shared.Common;
using CodeLab.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CodeLab.Application.Shared.Extensions;

public static class ServiceRegisrationExtensions
{
    private sealed record ServiceTypes(Type Interface, Type Implementation);

    private static IEnumerable<ServiceTypes> GetServiceTypesFromAssembly(Assembly assembly, Type genericInterfaceType)
    {
        return assembly.GetTypes()
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .SelectMany(type =>
                type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterfaceType)
                    .Select(i => new ServiceTypes(i, type)));
    }

    private static void AddScopedServices(this IServiceCollection services, Assembly assembly, Type genericInterfaceType)
    {
        foreach (var service in GetServiceTypesFromAssembly(assembly, genericInterfaceType))
        {
            services.AddScoped(service.Interface, service.Implementation);
        }
    }

    private static void AddValidationRegister(this IServiceCollection services)
    {
        var assembly = typeof(ServiceRegisrationExtensions).Assembly;
        services.AddScopedServices(assembly, typeof(IValidator<>));
    }

    public static IServiceCollection AddDiscoveryServices(this IServiceCollection services, Assembly[] assemblies)
    {
        services.AddValidationRegister();

        var excluded = new[]
        {
            typeof(IDomainEvent),
            typeof(IRequest<>),
            typeof(ICommand<>),
            typeof(INotification)
        };

        var allTypes = assemblies.SelectMany(a => a.GetTypes()).ToList();

        var interfaces = allTypes
            .Where(t => t.IsInterface && !excluded.Contains(t))
            .ToList();

        var classes = allTypes
            .Where(t => t.IsClass && !t.IsAbstract)
            .ToList();

        foreach (var iface in interfaces)
        {
            var realImplementations = new List<Type>();

            foreach (var impl in classes)
            {
                if (impl.GetCustomAttribute<FallbackAttribute>() != null)
                    continue;

                if (ImplementsInterface(impl, iface))
                    realImplementations.Add(impl);
            }

            foreach (var impl in realImplementations)
            {
                Register(services, iface, impl);
            }

            if (realImplementations.Count == 0)
            {
                var fallback = classes.FirstOrDefault(c =>
                    c.GetCustomAttribute<FallbackAttribute>() != null &&
                    ImplementsInterface(c, iface));

                if (fallback != null && !services.Any(sd => sd.ServiceType == iface))
                {
                    Register(services, iface, fallback);
                }
            }
        }

        return services;
    }

    private static bool ImplementsInterface(Type implementation, Type iface)
    {
        if (!iface.IsGenericTypeDefinition)
        {
            return iface.IsAssignableFrom(implementation);
        }

        return implementation
            .GetInterfaces()
            .Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == iface);
    }

    private static void Register(IServiceCollection services, Type iface, Type impl)
    {
        if (iface.IsGenericTypeDefinition)
        {
            if (impl.IsGenericTypeDefinition)
            {
                services.AddScoped(iface, impl);
            }
            else
            {
                var closedInterface = impl
                    .GetInterfaces()
                    .First(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == iface);

                services.AddScoped(closedInterface, impl);
            }
        }
        else
        {
            services.AddScoped(iface, impl);
        }
    }
}