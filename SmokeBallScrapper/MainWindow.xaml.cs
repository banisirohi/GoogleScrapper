using Microsoft.Extensions.Logging;
using SmokeBallScrapper.Model.Scrapper;
using System;
using System.Windows;

namespace SmokeBallScrapper
{
    /// <summary>
    /// Scrapper Window
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IScrapper scrapper;
        private readonly ILogger logger;
        public MainWindow(IScrapper _googleScrapper, ILogger<MainWindow> _logger)
        {
            InitializeComponent();
            scrapper = _googleScrapper;
            logger = _logger;
        }

        /// <summary>
        /// Scrapper call handler. Calls StartScrapingAsync to start scrapper.
        /// </summary>
        private async void BtnStartScrapper_Click(object sender, RoutedEventArgs e)
        {
            btnScrapper.IsEnabled = false;
            try
            {
                var _scrapperInput = this.DataContext as ScrapperInputViewModel;
                var result = await scrapper.StartScrapingAsync(_scrapperInput.Keywords, _scrapperInput.URL, _scrapperInput.MaxLookupRecords ?? 0);
                var occurances = result.Status != Errors.CaptchaFoundException && result?.Output?.Count > 0 ? string.Join(",", result.Output) : "0";
                _scrapperInput.Outcome = $"{result.Status}: {occurances}";
            }
            catch(Exception err)
            {
                logger.LogError(err.Message);
            }
            btnScrapper.IsEnabled = true;
        }
    }
}
