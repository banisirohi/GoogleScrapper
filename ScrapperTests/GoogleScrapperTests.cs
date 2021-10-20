using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SmokeBallScrapper.Model.Scrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrapperTests
{
    /// <summary>
    /// Unit test to validate Google scrapper operation
    /// </summary>
    [TestFixture]
    public class GoogleScrapperTests
    {
        private GoogleScapper _googleScapper;

        /// <summary>
        /// Setting up initial dependencies
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var logger = Mock.Of<ILogger<SmokeBallScrapper.MainWindow>>();
            _googleScapper = new GoogleScapper(logger);
        }

        /// <summary>
        /// Checks if input values are invalid.
        /// </summary>
        [TestCase("conveyancing software", "", 0)]
        [TestCase("conveyancing software", "43543", 10)]
        [TestCase("conveyancing software", "www.smokeball.com.au", 0)]
        public async Task IsNotValid_Input_ReturnFalseAsync(string keywords, string url, int lookup)
        {
            ValidInputStatus result = await _googleScapper.ValidateInputsAsync(keywords, url, lookup);
            Assert.IsFalse(result.Flag);
        }

        /// <summary>
        /// Checks if input values are valid
        /// </summary>
        [TestCase("conveyancing software", "www.smokeball.com.au", 10)]
        [TestCase("women clothing", "https://www.moochi.co.nz/", 100)]
        public async Task IsValid_Input_ReturnTrueAsync(string keywords, string url, int lookup)
        {
            ValidInputStatus result = await _googleScapper.ValidateInputsAsync(keywords, url, lookup);
            Assert.IsTrue(result.Flag);
        }

        /// <summary>
        /// Checks if scrapper return success result on valid input
        /// </summary>
        [TestCase("conveyancing software", "www.smokeball.com.au")]
        [TestCase("women clothing", "www.mode.co.nz")]
        public async Task IsValid_Input_ReturnListAsync(string keywords, string url)
        {
            ScrapperOutput result = await _googleScapper.StartScrapingAsync(keywords, url, 100);
            Assert.That(result.Output.FirstOrDefault(), Is.Not.EqualTo(0));
        }

        /// <summary>
        /// Checks if scrapper return empty result on invalid input
        /// </summary>
        [TestCase("conveyancing software", "")]
        [TestCase("conveyancing software", "43543")]
        [TestCase(" ", "www.smokeball.com.au")]
        public async Task IsNotValid_Input_ReturnEmptyListAsync(string keywords, string url)
        {
            ScrapperOutput result = await _googleScapper.StartScrapingAsync(keywords, url, 100);
            Assert.IsNull(result.Output);
        }
    }
}
