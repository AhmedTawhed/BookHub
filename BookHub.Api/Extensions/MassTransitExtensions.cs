using MassTransit;

namespace BookHub.Api.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddBookHubMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            var host = configuration["RabbitMQ:Host"];
            var useRabbitMq = !string.IsNullOrWhiteSpace(host) && host != "localhost";

            if (useRabbitMq)
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(host, "/", h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]!);
                        h.Password(configuration["RabbitMQ:Password"]!);
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
            }
            else
            {
                x.UsingInMemory((ctx, cfg) => cfg.ConfigureEndpoints(ctx));
            }
        });

        return services;
    }
}
