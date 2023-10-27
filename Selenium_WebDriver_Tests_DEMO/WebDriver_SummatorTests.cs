using OpenQA;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumWebDriverTests
{
    public class WebDriver_SummatorTests
    {
        IWebDriver driver;

        [OneTimeSetUp]
        public void OpenBrowserAndPageNavigate()
        {
            driver = new ChromeDriver();
            driver.Url = "https://sum-numbers.nakov.repl.co/";
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void Test_SumTwoPositiveNumbers()
        {
            driver.FindElement(By.CssSelector("input[name='number1']")).SendKeys("5");
            driver.FindElement(By.CssSelector("input[name='number2']")).SendKeys("4");
            driver.FindElement(By.CssSelector("input#calcButton")).Click();
            var result = driver.FindElement(By.CssSelector("#result")).Text;

            Assert.That(result, Is.EqualTo("Sum: 9"));
            driver.FindElement(By.Id("resetButton")).Click();
        }

        [Test]
        public void Test_SumTwoNegativeNumbers()
        {
            driver.FindElement(By.CssSelector("input[name='number1']")).SendKeys("-5");
            driver.FindElement(By.CssSelector("input[name='number2']")).SendKeys("-1");
            driver.FindElement(By.CssSelector("input#calcButton")).Click();
            var result = driver.FindElement(By.CssSelector("#result")).Text;

            Assert.That(result, Is.EqualTo("Sum: -6"));
            driver.FindElement(By.Id("resetButton")).Click();
        }

        [Test]
        public void Test_PositiveAndNegativeNumbers()
        {
            driver.FindElement(By.CssSelector("input[name='number1']")).SendKeys("5");
            driver.FindElement(By.CssSelector("input[name='number2']")).SendKeys("-1");
            driver.FindElement(By.CssSelector("input#calcButton")).Click();
            var result = driver.FindElement(By.CssSelector("#result")).Text;

            Assert.That(result, Is.EqualTo("Sum: 4"));
            driver.FindElement(By.Id("resetButton")).Click();
        }

        [Test]
        public void Test_EmptyFields()
        {
            driver.FindElement(By.CssSelector("input#calcButton")).Click();
            var result = driver.FindElement(By.CssSelector("#result")).Text;

            Assert.That(result, Is.EqualTo("Sum: invalid input"));
            driver.FindElement(By.Id("resetButton")).Click();
        }

        [Test]
        public void Test_IncorrectFields()
        {
            driver.FindElement(By.Id("number1")).SendKeys("Incorrect");
            driver.FindElement(By.Id("number2")).SendKeys("fileds");
            driver.FindElement(By.CssSelector("input#calcButton")).Click();
            var result = driver.FindElement(By.CssSelector("#result")).Text;

            Assert.That(result, Is.EqualTo("Sum: invalid input"));
            driver.FindElement(By.Id("resetButton")).Click();
        }

        [Test]
        public void Test_ResetButton()
        {

            driver.FindElement(By.CssSelector("input[name='number1']")).SendKeys("1");
            driver.FindElement(By.CssSelector("input[name='number2']")).SendKeys("2");
            driver.FindElement(By.CssSelector("input#calcButton")).Click();

            var firstField = driver.FindElement(By.CssSelector("input[name='number1']")).GetAttribute("value");
            Assert.IsNotEmpty(firstField);
            var secondField = driver.FindElement(By.CssSelector("input[name='number2']")).GetAttribute("value");
            Assert.IsNotEmpty(secondField);

            driver.FindElement(By.Id("resetButton")).Click();
            var firstFieldEmpty = driver.FindElement(By.CssSelector("input[name='number1']")).GetAttribute("value");
            Assert.IsEmpty(firstFieldEmpty);
            var secondFieldEmpty = driver.FindElement(By.CssSelector("input[name='number2']")).GetAttribute("value"); ;
            Assert.IsEmpty(secondFieldEmpty);
        }

        [OneTimeTearDown]
        public void CloseBrowser()
        {
            driver.Quit();
        }
    }
}