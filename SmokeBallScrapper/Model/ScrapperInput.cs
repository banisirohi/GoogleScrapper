using Prism.Mvvm;

namespace SmokeBallScrapper
{
    /// <summary>
    /// ViewModel for Scrapper.
    /// </summary>
    public class ScrapperInputViewModel : BindableBase
    {
        #region Properties
        private string keywords = "conveyancing software";
        private string url= "www.smokeball.com.au";
        private int? maxLookupRecords = 100;
        private string outcome;

        public string Keywords
        {
            get => keywords;
            set => SetProperty(ref keywords, value);
        }
        public string URL
        {
            get => url;
            set => SetProperty(ref url, value);
        }
        public int? MaxLookupRecords
        {
            get => maxLookupRecords;
            set => SetProperty(ref maxLookupRecords, value);
        }
        public string Outcome
        {
            get => outcome;
            set => SetProperty(ref outcome, value);
        }
        #endregion
    }
}
