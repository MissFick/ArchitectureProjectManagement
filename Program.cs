﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ArchitectureProjectManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            var host = hostBuilder.Build();
            
            using (var scope = host.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider; 
                try
                {
                    EnsureDataStorageIsReady(scopedServices);

                }
                catch (Exception ex)
                {
                    var logger = scopedServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            var env = host.Services.GetRequiredService<IWebHostEnvironment>();
            var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
            var config = host.Services.GetRequiredService<IConfiguration>();
            ConfigureLogging(env, loggerFactory, host.Services, config);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void EnsureDataStorageIsReady(IServiceProvider scopedServices)
        {
            var deleteLogsOlderThanDays = 90;
            LoggingEFStartup.InitializeDatabaseAsync(scopedServices, deleteLogsOlderThanDays).Wait();
            CoreEFStartup.InitializeDatabaseAsync(scopedServices).Wait();
            KvpEFCoreStartup.InitializeDatabaseAsync(scopedServices).Wait();
            CloudscribeIdentityServerIntegrationEFCoreStorage.InitializeDatabaseAsync(scopedServices).Wait();
            DynamicPolicyEFCore.InitializeDatabaseAsync(scopedServices).Wait();

            

            
        }

        private static void ConfigureLogging(
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider,
            IConfiguration config
            )
        {
            var dbLoggerConfig = config.GetSection("DbLoggerConfig").Get<cloudscribe.Logging.Models.DbLoggerConfig>();
            LogLevel minimumLevel;
            string levelConfig;
            if (env.IsProduction())
            {
                levelConfig = dbLoggerConfig.ProductionLogLevel;
            }
            else
            {
                levelConfig = dbLoggerConfig.DevLogLevel;
            }
            switch (levelConfig)
            {
                case "Debug":
                    minimumLevel = LogLevel.Debug;
                    break;

                case "Information":
                    minimumLevel = LogLevel.Information;
                    break;

                case "Trace":
                    minimumLevel = LogLevel.Trace;
                    break;

                default:
                    minimumLevel = LogLevel.Warning;
                    break;
            }
            
            // a customizable filter for logging
            // add exclusions in appsettings.json to remove noise in the logs
            bool logFilter(string loggerName, LogLevel logLevel)
            {
                if (dbLoggerConfig.ExcludedNamesSpaces.Any(f => loggerName.StartsWith(f)))
                {
                    return false;
                }

                if (logLevel < minimumLevel)
                {
                    return false;
                }

                if (dbLoggerConfig.BelowWarningExcludedNamesSpaces.Any(f => loggerName.StartsWith(f)) && logLevel < LogLevel.Warning)
                {
                    return false;
                }
                return true;
            }

            loggerFactory.AddDbLogger(serviceProvider, logFilter);
        }

    }


}
