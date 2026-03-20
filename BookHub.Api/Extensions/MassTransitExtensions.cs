using MassTransit;

namespace BookHub.Api.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddBookHubMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"], "/", h =>
                {
                    h.Username(configuration["RabbitMQ:Username"]!);
                    h.Password(configuration["RabbitMQ:Password"]!);
                });

                cfg.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}
