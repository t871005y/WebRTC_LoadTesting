using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRTC_LoadTesting
{
    internal class SeleniumTestService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IConfiguration _config;
        public SeleniumTestService(
            ILogger<SeleniumTestService> logger,
            IHostApplicationLifetime appLifetime,
            IConfiguration config)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            List<Task> tasks = new List<Task>();
            int workerCount = _config.GetValue<int>("worker");
            int tabCount = _config.GetValue<int>("tab");
            bool useGrid = _config.GetValue<bool>("grid:enabled");
            bool fakeStream = _config.GetValue<bool>("fake-stream");
            bool headless = _config.GetValue<bool>("headless");
            string grid_url = _config.GetValue<string>("grid:url");
            for (int a = 0; a < workerCount; a++)
            {
                //string url = "https://shuttle.tempestdigi.com/test?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjb250ZXh0Ijp7InVzZXIiOnsibmFtZSI6IllDX0xpbiIsImlkIjoiYWJjZDphMWIyYzMtZDRlNWY2LTBhYmMxLTIzZGUtYWJjZGVmMDFmZWRjYmEifSwiZ3JvdXAiOiJhMTIzLTEyMy00NTYtNzg5In0sImF1ZCI6ImppdHNpIiwiaXNzIjoic2h1dHRsZV9hcHBfaWQiLCJzdWIiOiJtZWV0LmppdC5zaSIsInJvb20iOiIqIiwiZXhwIjo5NTAwMDA2OTIzfQ.p-IiM1P7W1PaW4GwE63c6fxmVMMBfAQFoZXKz4ZBO3I";
                string url = _config.GetValue<string>("url");
                var t = SeleniumTest.Main(url, tabCount, fakeStream, cancellationToken, headless, useGrid, grid_url);
                tasks.Add(t);
            }
            await Task.WhenAll(tasks);
        }
    }
}
