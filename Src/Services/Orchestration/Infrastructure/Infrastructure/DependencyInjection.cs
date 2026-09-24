using Domain.StateData;
using Infrastructure;
using Infrastructure.StateMachine;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Orchestration.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var rabbitMqConnectionString =
            configuration.GetConnectionString("messaging");

        if (string.IsNullOrWhiteSpace(rabbitMqConnectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'messaging' was not configured. " +
                "Expected environment variable: ConnectionStrings__messaging");
        }

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            // ثبت State Machine
            x.AddSagaStateMachine<TicketStateMachine, TicketStateData>()
                .EntityFrameworkRepository(r =>
                {
                    r.ConcurrencyMode = ConcurrencyMode.Pessimistic; // یا Optimistic
                    r.AddDbContext<DbContext, OrchestrationDbContext>((provider, builder) =>
                    {
                        builder.UseSqlServer(
                            configuration.GetConnectionString("OrchestrationConnection"));
                    });
                });

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(rabbitMqConnectionString));

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
