using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SmokeBallScrapper
{
    /// <summary>
    /// Perform scrapper operations using HtmlAgilityPack
    /// </summary>
    public static class HtmlAgilityScraperType
    {
        /// <summary>
        /// Gets html document
        /// </summary>
        /// <param name="url">google search url</param>
        public static HtmlDocument GetDocument(string url)
        {
            HtmlWeb scraper = new();
            return scraper.Load(url);
        }

        /// <summary>
        /// Gets list of positions where searchUrl occured in htmlDocument.
        /// Works as extention method with htmlDocument object.
        /// </summary>
        /// <param name="htmlDocument">html document to be parsed.</param>
        /// <param name="expr">html node element to select from html document</param>
        /// <param name="maxLookupRecords">top n number of nodes to select from html document</param>
        /// <param name="searchUrl">string to search in html document</param>
        public static IList<int> GetOccurances(this HtmlDocument htmlDocument, string expr, int maxLookupRecords, string searchUrl)
        {
            var searchUri = new UriBuilder(searchUrl);
            IList<int> occurances = new List<int>();
            var nodes = htmlDocument.DocumentNode.SelectNodes(expr).Take(maxLookupRecords) ?? Enumerable.Empty<HtmlNode>();
            occurances = nodes.Select((node, index) => new { node, index })
                .Where(w => w.node.InnerText.Contains(searchUri.Host))
                .Select(s => s.index + 1).ToList();
            return occurances;
        }

        /// <summary>
        /// Checks if htmlDocument is a google's captcha form.
        /// Returns true if captcha present. False otherwise.
        /// </summary>
        public static bool IsCaptchaPage(this HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectNodes("//*[@id='captcha-form']") != null;
        }
    }
}
