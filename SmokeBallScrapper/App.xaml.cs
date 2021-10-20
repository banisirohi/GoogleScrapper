using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmokeBallScrapper.Model.Scrapper;
using System.IO;
using System.Windows;

namespace SmokeBallScrapper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider serviceProvider;
        public App()
        {
            ServiceCollection services = new();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }
        private void ConfigureServices(ServiceCollection services)
        {
            var path = Directory.GetCurrentDirectory();
            services.AddScoped<IScrapper, GoogleScapper>();
            // Adding file logging.
            services.AddSingleton<MainWindow>().AddLogging(configure => configure.AddFile($"{path}\\Logs\\Log.txt"));
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
