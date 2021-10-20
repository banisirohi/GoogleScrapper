using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokeBallScrapper
{
    /// <summary>
    /// Handles logging.
    /// </summary>
    public class ScrapperLogger
    {
        private readonly ILoggerFactory _logger;

        public ScrapperLogger()
        {
        }

        public ScrapperLogger(ILoggerFactory logger)
        {
            _logger = logger;
        }
    }
}
