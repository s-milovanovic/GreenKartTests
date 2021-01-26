using GreenKartTests.Utils;
using OpenQA.Selenium;

namespace GreenKartTests.Pages
{
    public class BasicPage : WebDriver
    {     
        public IWebElement Find(string xpath = "", string className = "", int timeout = 0)
        {
            return FindElement(xpath, className, timeout);
        }                 
    }
}
