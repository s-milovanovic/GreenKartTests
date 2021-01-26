using GreenKartTests.Reporter;
using GreenKartTests.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace GreenKartTests.Pages
{
    public class GreenKartBasicPage : BasicPage
    {
        const string GreenKartUrl = "https://rahulshettyacademy.com/seleniumPractise/#/";
        private IWebElement SuccessMsg => Find(className: "wrapperTwo");
        private IWebElement SiteLogo => Find(className: "greenLogo", timeout: 5);

        public void OpenWebsite()
        {
            OpenUrl(GreenKartUrl);
        }

        internal string ReadMessageStatus()
        {
            Report.Info("Success message should appear...");
            return ElementExt.ReadContent(SuccessMsg);
        }

        internal bool GreenLogoIsDisplayed()
        {
            try
            {
                return SiteLogo.Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}
