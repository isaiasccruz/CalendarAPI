using CalendarAPI.Application;
using CalendarAPI.Application.Interfaces;
using CalendarAPI.Infrastructure;
using CalendarAPI.Infrastructure.Interfaces;
using CalendarAPI.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace CalendarAPI.CrossCutting.IoC
{
    [ExcludeFromCodeCoverage]
    public static class DependencyResolver
    {
        private static readonly LogEventLevel _defaultLogLevel = LogEventLevel.Information;
        private static readonly LoggingLevelSwitch _loggingLevel = new LoggingLevelSwitch();

        public static void AddDependencyResolver(this IServiceCollection services)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            HttpClient httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromMinutes(1)
            };
            services.AddSingleton(httpClient);

            services.AddScoped<IUtilRepository, UtilRepository>();
            services.AddScoped<IDatabase, DapperConnection>();

            RegisterApplications(services);
            RegisterRepositories(services);
            AddLoggingSerilog();
        }

        private static void AddLoggingSerilog()
        {

            string configLogLevel = Environment.GetEnvironmentVariable("LOG_LEVEL") ?? _defaultLogLevel.ToString();
            bool parsed = Enum.TryParse(configLogLevel, true, out LogEventLevel logLevel);
            _loggingLevel.MinimumLevel = parsed ? logLevel : _defaultLogLevel;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.ControlledBy(_loggingLevel)
                .CreateLogger();
        }

        private static void RegisterApplications(IServiceCollection services)
        {
            services.AddScoped<ICalendarApplication, CalendarApplication>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<ICalendarRepository, CalendarRepository>();
        }
    }
}