using Deferat.Models;
using Microsoft.Extensions.Logging;

namespace Deferat.Services
{
    public class SiteInfo : ISiteInfo
    {
        private ILogger _logger;
        private IFileReader<Settings> _fileReader;

        public string BasePath { get;}
        public Settings Settings {get;}

        public SiteInfo(string path, ILogger<SiteInfo> logger, IFileReader<Settings> fileReader)
        {
            BasePath = path;
            _logger = logger;
            _fileReader = fileReader;

            _logger.LogInformation($"Loading settings from {path}...");

            Settings = _fileReader.ReadFile($"{path}/index.md");
        }

    }
}