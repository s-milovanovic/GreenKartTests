using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Linq;
using System.IO;
using AventStack.ExtentReports.Reporter.Configuration;
using GreenKartTests.Utils;

namespace GreenKartTests.Reporter
{
    //http://extentreports.com/docs/versions/4/net/

    public static class Report
    {
        public static AventStack.ExtentReports.ExtentReports ReportManager { get; set; }

        public static string FullPath { get; set; }

        private static string Report_filename { get; set; }

        internal static void SaveReport()
        {
            ReportManager.Flush();

            if (Consts.FixReportTimestamp)
                Timestamp.RunFix(Report_filename);
        }

        private static ExtentTest CurrentTestCase { get; set; } = null;

        public static void StartReporter()
        {
            CreateReportDirectory();

            //if (!Reload()) return;

            DeleteImagesFromDirectory();

            Report_filename = Path.Combine(FullPath, "index.html");

            var htmlReporter = new ExtentHtmlReporter(Report_filename);
            //var htmlReporter = new ExtentHtmlReporter(filename); // index.html

            htmlReporter.Config.Theme = Theme.Dark;
            htmlReporter.Config.DocumentTitle = "GreenKart Tests Report";
            htmlReporter.Config.ReportName = "GreenKart UI Test Report";
            htmlReporter.Config.EnableTimeline = false;

            //htmlReporter.Config.SetTimeStampFormat .SetTimeStampFormat("mm/dd/yyyy hh:mm:ss a");

            ReportManager = new AventStack.ExtentReports.ExtentReports();
            ReportManager.AttachReporter(htmlReporter);
        }

        private static void DeleteImagesFromDirectory()
        {
            foreach (string file in Directory.GetFiles(FullPath, "*.jpg")
                .Where(item => item.EndsWith(".jpg")))
                File.Delete(file);
        }

        private static void CreateReportDirectory()
        {
            var path = Path.GetDirectoryName(Path.GetDirectoryName(
                    TestContext.CurrentContext.TestDirectory));

            FullPath = Path.Combine(path, "Report");

            if (!Directory.Exists(FullPath))
                Directory.CreateDirectory(FullPath);
        }

        public static void AddNewTest(string testname = "", string args = "")
        {
            var method = Helper.Humanize(testname);
            if (string.IsNullOrEmpty(method))
                method = Helper.Humanize(TestContext.CurrentContext.Test.MethodName);
            CurrentTestCase = ReportManager.CreateTest(method + args);
        }

        private static void TakeScreenShotToFile(string error)
        {
            CurrentTestCase.Fail(error);

            if (WebDriver.Driver == null) return;

            var screen = new ScreenshotTaker();
            if (screen.TakeScreenshotForFailure())
                CurrentTestCase.AddScreenCaptureFromPath(screen.FullFileName);
        }

        public static void ReportTestOutcome()
        {
            if (CurrentTestCase == null) return;

            var error = TestContext.CurrentContext.Result.Message;

            switch (TestContext.CurrentContext.Result.Outcome.Status)
            {
                case TestStatus.Failed:
                case TestStatus.Inconclusive:
                case TestStatus.Warning:
                    TakeScreenShotToFile(error);
                    break;
                case TestStatus.Skipped:
                    CurrentTestCase.Skip("Test skipped " + error);
                    break;
                default:
                    CurrentTestCase.Pass("Pass");
                    break;
            }

            if (Consts.SaveReportForEachTest)
                ReportManager.Flush();

            CurrentTestCase = null;
        }

        public static void Warning(string message)
        {
            CurrentTestCase.Log(Status.Warning, message);
        }

        public static void Info(string message)
        {

            CurrentTestCase.Log(Status.Info, message);
        }

        public static void Pass(string message)
        {
            CurrentTestCase.Log(Status.Pass, message);
        }
        public static void Fail(string message)
        {
            CurrentTestCase.Log(Status.Fail, message);
        }
        internal static bool Reload()
        {
            var filename = Path.Combine(FullPath, "report.txt");
            if (!File.Exists(filename))
                return true;

            var data = File.ReadAllLines(filename)
                .Select(x => x.Split('='))
                .Where(x => x.Length > 1)
                .ToDictionary(x => x[0].Trim(), x => x[1]);

            return data["reload"] == "yes" ? true : false;
        }
    }
}
