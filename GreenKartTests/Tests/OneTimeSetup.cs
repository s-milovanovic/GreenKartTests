using GreenKartTests.Reporter;
using GreenKartTests.Utils;
using NUnit.Framework;

namespace GreenKartTests.Tests
{
    [SetUpFixture]
    public class OneTimeSetup
    {
        [OneTimeSetUp]
        public void GlobalOneTimeSetup()
        {
            Report.StartReporter();     
        }

        [OneTimeTearDown]
        public void GlobalOneTimeTearDown()
        {
            WebDriver.CloseAndQuit();
            Report.SaveReport();
        }
    }
}