using GreenKartTests.Reporter;
using GreenKartTests.Utils;
using NUnit.Framework;
using OpenQA.Selenium;
using System;

namespace GreenKartTests.Pages
{
    public class GreenKartPlaceOrderPage : GreenKartBasicPage
    {
        private IWebElement TermsAndConditions => Find(className: "chkAgree");
        private IWebElement DropDownMenu => Find("//div[@class='products']//div//div//select");
        private IWebElement ProceedButton => Find("//button[text()='Proceed']");

        internal void SelectCountry()
        {
            string country = "Serbia";
            SelectFromDropDown(country);
            Report.Info($"Following country is selected: {country}");
            TestContext.Out.WriteLine($"Following country is selected: {country}");
        }

        private void SelectFromDropDown(string country)
        {
            ElementExt.ClickOnElement(DropDownMenu);
            ElementExt.ClickOnElement(Find($"//option[@value = '{country}']"));
        }

        internal void AcceptTermsAndConditions()
        {
            ElementExt.ClickOnElement(TermsAndConditions);
            Report.Info("Terms & Conditions are accepted");
        }
        internal bool CompletePurchase()
        {
            string msg = "Thank you, your order has been placed successfully" + Environment.NewLine +
                "You'll be redirected to Home page shortly!!";
            ElementExt.ClickOnElement(ProceedButton);

            if(ReadMessageStatus().Equals(msg))
            {
                Report.Pass("The purchase process is successfully completed!");
                TestContext.Out.WriteLine("The purchase process is successfully completed!");
                return true;
            }
            else
            {
                Report.Fail("The purchase process has failed to complete!");
                TestContext.Out.WriteLine("The purchase process has failed to complete!");
                return false;
            }
        }
    }
}
