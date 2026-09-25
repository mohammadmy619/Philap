using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Instrumentation.EntityFrameworkCore;
using OpenTelemetry.Trace;

namespace Trip.Api.Extensions;

public static class OpenTelemetryExtensions
{
    private const string HealthEndpointPath = "/health";
    private const string AlivenessEndpointPath = "/alive";

    public static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        // 1. Logging
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        // 2. OpenTelemetry Configuration
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource
                    .AddService(
                        serviceName: builder.Environment.ApplicationName,
                        serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0",
                        serviceInstanceId: Environment.MachineName)
                    .AddAttributes(new[]
                    {
                        new KeyValuePair<string, object>("deployment.environment", builder.Environment.EnvironmentName)
                    });
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation() // در صورت نیاز به بررسی مصرف رم و CPU پروسس
                    .AddMeter("Microsoft.EntityFrameworkCore") // EF Core 9+ native metrics
                    .AddMeter("MassTransit"); // MassTransit Metrics (کانسومرها، صف‌ها و ...)
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddSource(builder.Environment.ApplicationName)
                    .AddSource("MassTransit") // توزیع تریس‌ها در بستر RabbitMQ / Message Bus

                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.Filter = context =>
                            !context.Request.Path.StartsWithSegments(HealthEndpointPath)
                            && !context.Request.Path.StartsWithSegments(AlivenessEndpointPath);
                    })
                    .AddHttpClientInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })
                    .AddEntityFrameworkCoreInstrumentation(options =>
                    {
                        options.EnrichWithIDbCommand = (activity, command) =>
                        {
                            activity.SetTag("db.statement", command.CommandText);
                        };
                    
                    })
                    .AddGrpcClientInstrumentation();
            });

        // 3. Exporters
        builder.AddOpenTelemetryExporters();

        return builder;
    }

    private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];

        if (!string.IsNullOrWhiteSpace(otlpEndpoint))
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        // if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
        // {
        //     builder.Services.AddOpenTelemetry().UseAzureMonitor();
        // }

        return builder;
    }
}
