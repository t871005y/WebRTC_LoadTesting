using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRTC_LoadTesting
{
    internal class SeleniumTest
    {
        public static async Task Main(string url, int tab, bool fakeStream, CancellationToken cancellationToken, bool headless = true, bool useGrid = false, string grid_url = null)
        {
            await Task.Delay(25);

            ChromeOptions chromeOptions = new ChromeOptions();

            if (fakeStream)
            {
                //設定假串流進去
                chromeOptions.AddArgument("--use-fake-device-for-media-stream");
                //允許Media
                chromeOptions.AddArgument("--use-fake-ui-for-media-stream");
            }
            if (headless)
                chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--mute-audio");
            chromeOptions.AddArgument("log-level=3");
            IWebDriver driver = null;
            try
            {
                if (useGrid)
                    driver = new RemoteWebDriver(new Uri(grid_url), chromeOptions.ToCapabilities());
                else
                    driver = new ChromeDriver(chromeOptions);
                //IWebDriver driver = new RemoteWebDriver(new Uri("http://127.0.0.1:31891/wd/hub/"), chromeOptions.ToCapabilities());
                //IWebDriver driver = new RemoteWebDriver(new Uri("http://40.119.238.87:4444/wd/hub/"), chromeOptions.ToCapabilities());
            }
            catch (Exception e)
            {
                Console.WriteLine("error:" + e);
                return;
            }

            //driver.Navigate().GoToUrl(url);

            for (int t = 0; t < tab; t++)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript($"window.open(\"{url}\");");
                driver.SwitchTo().Window(driver.WindowHandles[driver.WindowHandles.Count - 1]);
                driver.FindElement(By.CssSelector("#videoconference_page > div.premeeting-screen > div:nth-child(1) > div > div > div.prejoin-input-area > div > div")).Click();
                //driver.SwitchTo().Window(driver.WindowHandles.Last());
                //driver.Navigate().GoToUrl(url);

            }
            driver.SwitchTo().Window(driver.WindowHandles[0]);
            driver.Close();

            while (true)
            {
                await Task.Delay(100);
                if (cancellationToken.IsCancellationRequested)
                {
                    //while (driver.WindowHandles.Count > 0)
                    //{
                    //    driver.SwitchTo().Window(driver.WindowHandles[0]);
                    //    driver.Close();
                    //}

                    driver.Quit();
                    Console.WriteLine("Stop");
                    return;
                }
            }
        }
    }
}
