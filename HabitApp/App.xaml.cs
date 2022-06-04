using HabitApp.Data;
using HabitApp.Implementation;
using HabitApp.Model;
using HabitApp.Services;
using HabitApp.View;
using HabitApp.VM;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace HabitApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public User CurrentUser { get; set; }

        private static IHost _Host;
        public static IHost Host
        {
            get
            {
                if (_Host == null)
                {
                    _Host = Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
                }
                return _Host;
            }
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Host;
            base.OnStartup(e);

            await host.StartAsync().ConfigureAwait(false);

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            LiveCharts.Configure(config =>
                config
                    // registers SkiaSharp as the library backend
                    // REQUIRED unless you build your own
                    .AddSkiaSharp()

                    // adds the default supported types
                    // OPTIONAL but highly recommend
                    .AddDefaultMappers()

                    // select a theme, default is Light
                    // OPTIONAL
                    //.AddDarkTheme()
                    .AddLightTheme()
                );
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            var host = Host;
            await host.StopAsync().ConfigureAwait(false);
            host.Dispose();
            _Host = null;
        }

        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddSingleton<UserRepository>();
            services.AddSingleton<HabitRepository>();
            services.AddSingleton<TaskRepository>();
            services.AddSingleton<DailyHabitRepository>();
            services.AddSingleton<StatisticsRepository>();

            services.AddTransient<LoginService>();
            services.AddTransient<AllHabitCRUDService>();
            services.AddTransient<AllHabitService>();
            services.AddTransient<StatisticsService>();

            services.AddSingleton<MainWindowVM>();
            services.AddTransient<HomeVM>();
            services.AddSingleton<LoginVM>();
            services.AddTransient<DashboardVM>();
            services.AddSingleton<CompletionRatingDialogVM>();

            services.AddTransient<HomeView>();
            services.AddTransient<LoginView>();
            services.AddTransient<DashboardView>();
            services.AddScoped<CompletionRatingDialog>();

            services.AddSingleton<PageNavigationManager>();
            services.AddSingleton<ICurrentDateTimeProvider, CurrentDateTimeProvider>();
        }
    }
}
