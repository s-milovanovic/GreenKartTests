using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GreenKartTests.Utils
{
    public class WebDriver
    {
        private static IWebDriver driver_variable = null;
        
        public static IWebDriver Driver
        {
            get
            {
                if (driver_variable == null)
                {
                    driver_variable = MakeChromeDriver();
                }
                return driver_variable;
            }
        }          
        private static IWebDriver MakeChromeDriver()
        {                
            ChromeOptions options = new ChromeOptions();                    
            options.AddArgument("ignore-certificate-errors");
            options.AddArgument("disable-popup-blocking");                    
            options.AddArgument("disable-infobars");
            options.AddArgument("disable-notifications");
            options.AddArgument("disable-extensions");                        
            options.AddArgument("no-sandbox");

            //options.AddUserProfilePreference("download.default_directory", Consts.DOWNLOAD_FOLDER);
            options.AddUserProfilePreference("download.prompt_for_download", false);   
            options.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);
            //options.AddUserProfilePreference("disable-popup-blocking", "true");
            //options.AddArgument("--headless");

            var result = new ChromeDriver(options);
            result.Manage().Window.Maximize();
            
            return result;
        }    

        internal static void CloseAndQuit()
        {
            if (Driver == null)
                return;

            Driver.Close();
            Driver.Quit();
            driver_variable = null;
        }

        internal static void OpenUrl(string url)
        {
            Driver.Navigate().GoToUrl(url);      
        }

        public static IWebElement FindAndWait(IWebDriver driver, string xpath, string className, int timeout)
        {         
            // Install-Package DotNetSeleniumExtras.WaitHelpers -Version 3.11.0

            if (timeout > 0)
            {
                //flient wait
                var fluentWait = new DefaultWait<IWebDriver>(driver)
                {
                    Timeout = TimeSpan.FromSeconds(timeout),
                    PollingInterval = TimeSpan.FromMilliseconds(250)
                };
                /* Ignore the exception - NoSuchElementException that indicates that the element is not present */
                fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                fluentWait.Message = "Element is not found";
                
                if (!String.IsNullOrEmpty(xpath))
                    return fluentWait.Until(x => x.FindElement(By.XPath(xpath)));
                return fluentWait.Until(x => x.FindElement(By.ClassName(className)));
                //standard excplicit wait
                /*var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(WebDriverTimeoutException));
                return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(xpath)));*/
            }
            if (!String.IsNullOrEmpty(xpath))
                return driver.FindElement(By.XPath(xpath));
            return driver.FindElement(By.ClassName(className));
        }

        public static IWebElement FindElement(string xpath, string className, int timeout)
        {
            return FindAndWait(Driver, xpath, className, timeout);
        }
    }
}
