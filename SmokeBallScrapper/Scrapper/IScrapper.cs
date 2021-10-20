using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmokeBallScrapper.Model.Scrapper
{
    /// <summary>
    /// Interface for Scrappers
    /// </summary>
    public interface IScrapper
    {
        Task<ValidInputStatus> ValidateInputsAsync(string keywords, string url, int lookup);
        Task<ScrapperOutput> StartScrapingAsync(string keywords, string url, int maxLookupRecords);
    }
    public record ScrapperOutput(string Status, IList<int> Output);
    public record ValidInputStatus(string Status, bool Flag);
}
