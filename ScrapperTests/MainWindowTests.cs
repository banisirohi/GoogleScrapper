using NUnit.Framework;
using System.Threading;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using SmokeBallScrapper;
using Moq;
using Microsoft.Extensions.Logging;
using SmokeBallScrapper.Model.Scrapper;

namespace ScrapperTests
{
    /// <summary>
    /// Integration test to validate databindings
    /// </summary>
    [TestFixture, Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        MainWindow view;
        ScrapperInputViewModel scrapperInputViewModel;

        /// <summary>
        /// Mocking and initialising dependencies for Scrapper MainWindow
        /// </summary>
        [SetUp]
        public void Setup()
        {
            var logger = Mock.Of<ILogger<MainWindow>>();
            var _googleScapper = new GoogleScapper(logger);
            view = new MainWindow(_googleScapper, logger);
            scrapperInputViewModel = new ScrapperInputViewModel();
            view.DataContext = scrapperInputViewModel;
            view.Show();
        }
        /// <summary>
        /// Clear dependencies and close automation
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            view.Close();
            view.DataContext = null;
            view = null;
            scrapperInputViewModel = null;
        }

        /// <summary>
        /// Tests to check if updating view Keyword textbox updated viewmodel.
        /// </summary>
        [Test]
        public void SetKeywordTextBox_NoAutomation()
        {
            string expected = "women clothing";
            SetValue(view.txtKeywords, expected);
            Assert.AreEqual(expected, scrapperInputViewModel.Keywords);
        }

        /// <summary>
        /// Tests to check if updating view Url textbox updated viewmodel.
        /// </summary>
        [Test]
        public void SetURLTextBox_NoAutomation()
        {
            string expected = "www.mode.co.nz";
            SetValue(view.txtURL, expected);
            Assert.AreEqual(expected, scrapperInputViewModel.URL);
        }

        /// <summary>
        /// Tests to check if initial bindings mapped to view.
        /// </summary>
        [Test]
        public void IsInitialTextBoxValueSet_NoAutomation()
        {
            Assert.AreEqual(view.txtKeywords.Text, scrapperInputViewModel.Keywords);
            Assert.AreEqual(view.txtURL.Text, scrapperInputViewModel.URL);
        }

        /// <summary>
        /// Perform automation to assign values to textbos.
        /// </summary>
        private static void SetValue(TextBox textbox, string value)
        {
            TextBoxAutomationPeer peer = new(textbox);
            IValueProvider provider = peer.GetPattern(PatternInterface.Value) as IValueProvider;
            provider.SetValue(value);
        }
    }
}