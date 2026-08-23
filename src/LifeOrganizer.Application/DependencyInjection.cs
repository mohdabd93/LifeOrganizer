using System.Reflection;
using FluentValidation;
using LifeOrganizer.Application.Common.Behaviours;
using LifeOrganizer.Application.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LifeOrganizer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped<IActivityConflictChecker, ActivityConflictChecker>();

        return services;
    }
}
