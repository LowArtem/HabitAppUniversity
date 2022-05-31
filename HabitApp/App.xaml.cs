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

namespace HabitApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        //public User CurrentUser { get; set; } = null;

        // Для тестов:
        public User CurrentUser { get; set; } = new User(7, "TrialBot", "TrialBot", 0, 0, null);

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

            services.AddTransient<LoginService>();
            services.AddTransient<AllHabitCRUDService>();
            services.AddTransient<AllHabitService>();

            services.AddSingleton<MainWindowVM>();
            services.AddSingleton<HomeVM>();
            services.AddSingleton<LoginVM>();
            services.AddSingleton<DashboardVM>();
            services.AddSingleton<CompletionRatingDialogVM>();

            services.AddTransient<HomeView>();
            services.AddScoped<LoginView>();
            services.AddScoped<DashboardView>();
            services.AddTransient<CompletionRatingDialog>();

            services.AddSingleton<PageNavigationManager>();
            services.AddSingleton<ICurrentDateTimeProvider, CurrentDateTimeProvider>();
        }
    }
}
