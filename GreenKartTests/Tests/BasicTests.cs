using GreenKartTests.Pages;
using GreenKartTests.Reporter;
using NUnit.Framework;

namespace GreenKartTests.Tests
{
    public class BasicTests : GreenKartBasicPage
    {
        protected bool VerifyWebsiteIsLoaded()
        {
            if (GreenLogoIsDisplayed())
            {
                Report.Pass("GREENKART Website is successfully loaded");
                return true;
            }
            else
            {
                Report.Fail("GREENKART Website has failed to load successfully");
                return false;
            }
            
        }

        [TearDown]
        public void TearDown()
        {
            Report.ReportTestOutcome();
        }
    }
}
