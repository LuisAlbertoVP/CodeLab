using CodeLab.Application.Identity.Interfaces;
using CodeLab.Application.Identity.Services;
using CodeLab.Application.Shared.Behavior;
using CodeLab.Application.Shared.Common;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CodeLab.Application.Shared.Extensions;

public static class ServiceRegisrationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = typeof(ServiceRegisrationExtensions).Assembly;

        var types = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract);

        var serviceTypes = new[]
        {
            typeof(IRequestHandler<,>),
            typeof(INotificationHandler<>),
            typeof(IValidator<>)
        };

        var registrations = types
            .SelectMany(type => type.GetInterfaces()
                .Where(i => i.IsGenericType && serviceTypes.Contains(i.GetGenericTypeDefinition()))
                .Select(i => new { Service = i, Implementation = type }));

        foreach (var reg in registrations)
        {
            services.AddScoped(reg.Service, reg.Implementation);
        }

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PerformanceMonitoringBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddScoped<IMediator, Mediator>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}