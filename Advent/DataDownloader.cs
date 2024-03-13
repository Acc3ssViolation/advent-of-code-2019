using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Advent
{
    internal class DataDownloader : IDisposable
    {
        private static readonly string DownloadDirectory = PathManager.DataDirectory;
        private const string BaseUrl = "https://adventofcode.com";
        private readonly string _sessionCookie;
        private readonly int _year;
        private HttpClient? _httpClient;
        private bool _disposed;

        public DataDownloader(string sessionCookie, int year)
        {
            _year = year;
            _sessionCookie = sessionCookie ?? throw new ArgumentNullException(nameof(sessionCookie));
        }

        public async Task DownloadDay(int day, CancellationToken cancellationToken)
        {
            var file = Path.Combine(DownloadDirectory, $"day{day:D2}.txt");
            if (File.Exists(file))
                return;

            Logger.Line($"Downloading data for day {day} of the {_year} AoC edition...");
            var http = GetClient();
            var response = await http.GetAsync($"{BaseUrl}/{_year}/day/{day}/input", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            using var fs = File.Create(file);
            await stream.CopyToAsync(fs, cancellationToken).ConfigureAwait(false);
            Logger.Line($"Saved to {file}");
        }

        private HttpClient GetClient()
        {
            if (_httpClient == null)
            {
                var cookieContainer = new CookieContainer();
                cookieContainer.Add(new Uri(BaseUrl), new Cookie("session", _sessionCookie));
                var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
                _httpClient = new HttpClient(handler, true);
            }
            return _httpClient;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_httpClient != null)
                    {
                        _httpClient.Dispose();
                        _httpClient = null;
                    }
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
