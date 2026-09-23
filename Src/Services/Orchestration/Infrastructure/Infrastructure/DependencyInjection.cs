using Infrastructure;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            // ۱. استفاده از فرمت نام‌گذاری استاندارد (اختیاری ولی شدیداً توصیه‌شده)
            x.SetKebabCaseEndpointNameFormatter();

            // ۲. ثبت تمام کانسیومرهای اسمبلی مورد نظر
            x.AddConsumers(typeof(IAssemblyMarker).Assembly);

            // ۳. کانفیگ هاست RabbitMQ و ساخت خودکار اندپوینت‌ها
            x.UsingRabbitMq((context, cfg) =>
            {
                if (Uri.TryCreate(
                        rabbitMqConnectionString,
                        UriKind.Absolute,
                        out var rabbitMqUri))
                {
                    cfg.Host(rabbitMqUri);
                }
                else
                {
                    cfg.Host(rabbitMqConnectionString, "/");
                }

                cfg.ConfigureEndpoints(context);
            });
        });


        return services;
    }
}
