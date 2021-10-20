using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokeBallScrapper
{
    /// <summary>
    /// Manages error messages
    /// </summary>
    public static class Errors
    {
        public const string InvalidScrapperInputException = "Invalid input value(s)";
        public const string CaptchaFoundException = "Capcha found. Try again after sometime";
    }

    /// <summary>
    /// Exception for invalid url string
    /// </summary>
    class URLNotFoundException : Exception
    {
        public URLNotFoundException() { }

        public URLNotFoundException(string value)
            : base($"Invalid URL: {value}")
        {

        }
    }
}
