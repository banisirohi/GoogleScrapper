using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using SmokeBallScrapper.Model.Scrapper;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmokeBallScrapper.Model.Scrapper
{
    /// <summary>
    /// Scrapper for Google search Engine for country code au.
    /// </summary>
    public class GoogleScapper : IScrapper
    {
        private const string baseURL = "https://www.google.com.au/search";
        private const string urlRegexPattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
        private readonly ILogger logger;
        public GoogleScapper(ILogger<MainWindow> _logger)
        {
            logger = _logger;
        }
        /// <summary>
        /// Gets number of occurance of url for keyword.
        /// </summary>
        /// <param name="keywords">keywords such as 'conveyancing software'</param>
        /// <param name="url">lookup url string such as 'www.smokeball.com.au'</param>
        /// <param name="maxLookupRecords">number of search results</param>
        public async Task<ScrapperOutput> StartScrapingAsync(string keywords, string url, int maxLookupRecords)
        {
            ScrapperOutput scrapperOutput = null;
            await Task.Run(async () =>
            {
                try
                {
                    ValidInputStatus validInputStatus = await ValidateInputsAsync(keywords, url, maxLookupRecords);
                    if (validInputStatus.Flag)
                    {
                        // Gets html of google searched results page
                        var page = HtmlAgilityScraperType.GetDocument($"{baseURL}?num={maxLookupRecords}&q={keywords.Replace(" ", "+")}");
                        // Check if valid search result present or captcha returned by google search
                        if (page.IsCaptchaPage())
                        {
                            scrapperOutput = new ScrapperOutput(Errors.CaptchaFoundException, null);
                            logger.LogInformation(Errors.CaptchaFoundException);
                        }
                        else
                        {
                            var pos = page.GetOccurances("//div[@class='g']", maxLookupRecords, url);
                            scrapperOutput = new ScrapperOutput("Results", pos);
                        }
                    }
                    else
                        scrapperOutput = new ScrapperOutput(validInputStatus.Status, null);
                }
                catch (Exception err)
                {
                    logger.LogError(err.Message);
                }
            });
            return scrapperOutput;
        }

        /// <summary>
        /// Validates scrapper input values.
        /// Returns ValidInputStatus with Flag true for valid and false for invalid data.
        /// </summary>
        /// <param name="keywords">keywords such as 'conveyancing software' should not be null or empty</param>
        /// <param name="url">lookup url string such as 'www.smokeball.com.au' should not be null or invalid url</param>
        /// <param name="maxLookupRecords">lookup should not be less than 1</param>
        public async Task<ValidInputStatus> ValidateInputsAsync(string keywords, string url, int lookup)
        {
            List<string> errMsgs = new();
            bool flag = true;
            await Task.Run(() =>
            {
                Regex UrlRgx = new(urlRegexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (string.IsNullOrEmpty(keywords.Trim())
                || string.IsNullOrEmpty(url.Trim())
                || lookup <= 0)
                {
                    errMsgs.Add(Errors.InvalidScrapperInputException);
                    flag = false;
                    logger.LogError(Errors.InvalidScrapperInputException);
                }

                if (!UrlRgx.IsMatch(url) && !string.IsNullOrEmpty(url))
                {
                    string urlErrMsg = (new URLNotFoundException(url)).ToString();
                    errMsgs.Add(urlErrMsg);
                    flag = false;
                    logger.LogError(urlErrMsg);
                }
            });
            return new ValidInputStatus(string.Join(" | ", errMsgs), flag);
        }
    }
}
