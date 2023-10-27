using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace URL_Shortener_Tests
{
    public class URLShortenerTests
    {
        public ChromeDriver driver;

        [SetUp]
        public void OpenBrowserAndNavigate()
        {
            driver = new ChromeDriver();
            driver.Url = "https://shorturl.nakov.repl.co/";
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Quit();
        }

        [Test]
        public void Test_HomePage()
        {
            var homePageButton = driver.FindElement(By.XPath("/html/body/header/a[1]"));
            homePageButton.Click();

            Assert.That(driver.Title, Is.EqualTo("URL Shortener"));
        }

        [Test]
        public void Test_ShortURLs_Page()
        {
            var ShortURLsButton = driver.FindElement(By.XPath("/html/body/header/a[2]"));
            ShortURLsButton.Click();

            Assert.That(driver.Title, Is.EqualTo("Short URLs"));

            var ShortURLsTitle = driver.FindElement(By.CssSelector("main > h1")).Text;
            Assert.That(ShortURLsTitle, Is.EqualTo("Short URLs"));

            var firstUrlVisits = driver.FindElement(By.XPath("/html/body/main/table/tbody/tr[1]/td[4]"));
            Assert.That(firstUrlVisits, Is.Not.Null);

            var firstCell = driver.FindElement(By.XPath("/html/body/main/table/tbody/tr[1]/td[1]/a")).Text;
            Assert.That(firstCell, Is.EqualTo("https://nakov.com"));

            var secondCell = driver.FindElement(By.XPath("/html/body/main/table/tbody/tr[1]/td[2]/a")).Text;
            Assert.That(secondCell, Is.EqualTo("http://shorturl.nakov.repl.co/go/nak"));
        }

        [Test]
        public void Test_AddURL_Page_InvalidData()
        {
            var addURLsPage = driver.FindElement(By.XPath("/html/body/header/a[3]"));
            addURLsPage.Click();

            var urlField = driver.FindElement(By.Id("url"));
            urlField.SendKeys("invalid data");

            var createButton = driver.FindElement(By.XPath("/html/body/main/form/table/tbody/tr[3]/td/button"));
            createButton.Click();

            var errorMessage = driver.FindElement(By.XPath("/html/body/div")).Text;

            Assert.That(errorMessage, Is.EqualTo("Invalid URL!"));
        }

        [Test]
        public void Test_AddURL_Page_ShortCodeField_Empty()
        {
            var addURLsPage = driver.FindElement(By.XPath("/html/body/header/a[3]"));
            addURLsPage.Click();

            var urlField = driver.FindElement(By.Id("url"));
            urlField.SendKeys("Test URL");

            var shortCodeField = driver.FindElement(By.Id("code"));
            shortCodeField.Clear();

            var createButton = driver.FindElement(By.XPath("/html/body/main/form/table/tbody/tr[3]/td/button"));
            createButton.Click();

            var errorMessage = driver.FindElement(By.XPath("/html/body/div")).Text;

            Assert.That(errorMessage, Is.EqualTo("Short code cannot be empty!"));
        }

        [Test]
        public void Test_AddURL_Page_URLCodeField_Empty()
        {
            var addURLsPage = driver.FindElement(By.XPath("/html/body/header/a[3]"));
            addURLsPage.Click();

            var urlField = driver.FindElement(By.Id("url"));
            urlField.Clear();

            var shortCodeField = driver.FindElement(By.Id("code"));
            shortCodeField.SendKeys("Test short code");

            var createButton = driver.FindElement(By.XPath("/html/body/main/form/table/tbody/tr[3]/td/button"));
            createButton.Click();

            var errorMessage = driver.FindElement(By.XPath("/html/body/div")).Text;

            Assert.That(errorMessage, Is.EqualTo("URL cannot be empty!"));
        }

        [Test]
        public void Test_AddURL_Page_ValidData()
        {
            var addURLsPage = driver.FindElement(By.XPath("/html/body/header/a[3]"));
            addURLsPage.Click();

            var urlField = driver.FindElement(By.Id("url"));
            var timeNow = DateTime.Now.Ticks;
            urlField.SendKeys("https://test" + timeNow + ".com");

            var shortCodeField = driver.FindElement(By.Id("code"));
            shortCodeField.Clear();
            var uniqueDateForShortCode = DateTime.Now.Ticks.ToString();
            shortCodeField.SendKeys(uniqueDateForShortCode);

            var cretateButton = driver.FindElement(By.XPath("/html/body/main/form/table/tbody/tr[3]/td/button"));
            cretateButton.Click();

            var table = driver.FindElements(By.CssSelector("table tr > td"));
            foreach (var item in table)
                if (item.Text.Contains(uniqueDateForShortCode))
                {
                    Assert.That(item.Text.Contains(uniqueDateForShortCode));
                    break;
                }
        }

        [Test]
        public void Test_VisitURL_Page()
        {
            var shortURLsPage = driver.FindElement(By.CssSelector("body > header > a:nth-child(3)"));
            shortURLsPage.Click();

            var oldPageVisits = driver.FindElement(By.CssSelector("tr:nth-child(2) > td:nth-child(4)")).Text;
            var oldPageVisitsInt = int.Parse(oldPageVisits);

            var seleniumShortLink = driver.FindElement(By.XPath("/html/body/main/table/tbody/tr[2]/td[2]/a"));
            seleniumShortLink.Click();

            driver.SwitchTo().Window(driver.WindowHandles[1]);

            Assert.That(driver.Title, Is.EqualTo("Selenium"));

            driver.SwitchTo().Window(driver.WindowHandles[0]);

            var actualPageVisits = driver.FindElement(By.CssSelector("tr:nth-child(2) > td:nth-child(4)")).Text;
            var actualPageVisitsInt = int.Parse(actualPageVisits);

            Assert.That(oldPageVisitsInt < actualPageVisitsInt);
        }
    }
}