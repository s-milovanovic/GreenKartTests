using GreenKartTests.Utils;
using NUnit.Framework;
using OpenQA.Selenium;

namespace GreenKartTests.Reporter
{
    public class ScreenshotTaker
    {
        private readonly string filename;

        public ScreenshotTaker()
        {
            filename = TestContext.CurrentContext.Test.MethodName + "_" + Helper.Guid();
        }

        public string FullFileName { get; private set; }
        
        public bool TakeScreenshotForFailure()
        {
            return TryToSaveScreenshot(GetScreenshot());
        }
        
        private bool TryToSaveScreenshot(Screenshot ss)
        {
            try
            {
                SaveScreenshot(filename, ss);
                return true;
            }
            catch
            {                
                return false;
            }
        }

        private void SaveScreenshot(string screenshotName, Screenshot ss)
        {
            if (ss == null)
                return;

            var filepath = $"{Report.FullPath}\\{screenshotName}.jpg";
            filepath = filepath.Replace('/', ' ').Replace('"', ' ');
            ss.SaveAsFile(filepath, ScreenshotImageFormat.Png);

            FullFileName = filepath;
        }

        private Screenshot GetScreenshot()
        {
            return ((ITakesScreenshot)WebDriver.Driver)?.GetScreenshot();
        }
    }
}