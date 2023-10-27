using OpenQA;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;

namespace Summator_DataDrivenTests
{
    public class SummatorDataDrivenTests
    {
        private ChromeDriver driver;

        [OneTimeSetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Url = "https://sum-numbers.nakov.repl.co/";
            driver.Manage().Window.Maximize();
        }

        [TestCase("5", "1", "Sum: 6")]
        [TestCase("15", "-1", "Sum: 14")]
        [TestCase("-5", "-11", "Sum: -16")]
        [TestCase("", "", "Sum: invalid input")]
        [TestCase("@", "2", "Sum: invalid input")]
        [TestCase("1", "test", "Sum: invalid input")]
        [TestCase(" ", "10", "Sum: invalid input")]
        [TestCase("15", "$", "Sum: invalid input")]
        [TestCase("TEXT", "100", "Sum: invalid input")]
        [TestCase("5.5", "2.3", "Sum: 7.8")]
        [TestCase("-154.5", "56.3", "Sum: -98.2")]
        [TestCase("-12", "-50.5", "Sum: -62.5")]
        [TestCase("-12.561", "-5.511153", "Sum: -18.072153")]

        public void Test_Summator(string num1, string num2, string result)
        {
            // Arrange
            var firstField = driver.FindElement(By.Id("number1"));
            var secondField = driver.FindElement(By.Id("number2"));
            var calcButton = driver.FindElement(By.Id("calcButton"));
            var resultField = driver.FindElement(By.Id("result"));
            var resetButton = driver.FindElement(By.Id("resetButton"));

            // Act
            firstField.SendKeys(num1);
            secondField.SendKeys(num2);
            calcButton.Click();

            string expectedResult = result;

            // Assert
            Assert.That(expectedResult, Is.EqualTo(resultField.Text));

            resetButton.Click();
        }

        [OneTimeTearDown]
        public void CloseBrowser()
        {
            driver.Quit();
        }
    }
}