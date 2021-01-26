using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;

namespace GreenKartTests.Utils
{
    public static class ElementExt
    {
        public static void ClickOnElement(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)WebDriver.Driver;
            js.ExecuteScript("arguments[0].click()", element);
        }

        public static string ReadContent(this IWebElement element)
        {
            //IJavaScriptExecutor js = (IJavaScriptExecutor)WebDriver.Driver;
            //return js.ExecuteScript("return arguments[0].textContent;", element).ToString();
            return element.ReadText();
        }        

        public static string ReadText(this IWebElement element)
        {
            //return element.GetAttribute("innerText"); // work for textContent also
            return element.Text;
            /*IJavaScriptExecutor js = (IJavaScriptExecutor)WebDriver.driver;
            return js.ExecuteScript("return arguments[0].innerText;", element).ToString();*/
        }
    }
}
