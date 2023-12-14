﻿using Bookinist.Data;
using Bookinist.Model;
using Bookinist.Model.AppSettings.AppConfig;
using Bookinist.Service;
using Bookinist.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using ProjectVersionInfo;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Bookinist
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();
        public static bool IsDesighnMode { get; private set; } = true;

        private static IHost _host;
        public static IHost Host => _host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs())
            .Build();

        public static IServiceProvider Services => Host.Services;

        protected override async void OnStartup(StartupEventArgs e)
        {
            var prjVersion = new ProjectVersion(Assembly.GetExecutingAssembly());
            _log.Debug($"Запуск приложения: {AppConst.Get().AppDesciption} {prjVersion.Version} билд от {prjVersion.BuildDate}");
            IsDesighnMode = false;
            var host = Host;

            using (var scope = Services.CreateScope())
                await scope.ServiceProvider.GetRequiredService<DBInitializer>().InitializeAsync(); //.Wait();

            base.OnStartup(e);
            await host.StartAsync().ConfigureAwait(false);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            var host = Host;
            await host.StopAsync().ConfigureAwait(false);
            host.Dispose();
            _host = null;
        }

        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
            .AddDatabase(host.Configuration.GetSection("Database"))
            .RegisterServices()
            .RegisterViewModels();

        public static string CurrentDirectory => IsDesighnMode
            ? Path.GetDirectoryName(GetSourceCodePath())
            : Environment.CurrentDirectory;

        private static string GetSourceCodePath([CallerFilePath] string Path = null) => Path;
    }
}