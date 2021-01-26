using HtmlAgilityPack;
using System;

namespace GreenKartTests.Reporter
{
    // Install-Package HtmlAgilityPack

    public static class Timestamp
    {
        private static HtmlDocument document = null;

        public static void RunFix(string report_filename)
        {
            document = new HtmlDocument();
            document.Load(report_filename);

            try
            {
                DeleteAllTimestamps();
                document.Save(report_filename);
            }
            catch
            {

            }
        }

        private static void DeleteAllTimestamps()
        {
            ReplaceData("//*[@id='category-view']//th[.='Timestamp']", "No.");

            ReplaceSpecialTimestampInfo();

            RemoveFromList("//div[@class='test-time-info']");
            RemoveFromList("//th[.='Timestamp']");
            RemoveFromList("//td[@class='timestamp']");
            RemoveFromList("//div[@class='col s2']");
            ReplaceData("//span[@class='label blue darken-3 suite-start-time']", DateTime.Now.ToShortDateString());

            ReplaceData("//span[@class='test-time']", "Status:");
        }

        private static void ReplaceSpecialTimestampInfo()
        {
            var key = ("//*[@id='category-view']/div[1]//td[1]");
            var list = document.DocumentNode.SelectNodes(key);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                    list[i].InnerHtml = (i + 1).ToString();
            }
        }
        private static void ReplaceData(string key, string new_value)
        {
            var list = document.DocumentNode.SelectNodes(key);
            if (list != null)
            {
                foreach (HtmlNode test in list)
                    test.InnerHtml = new_value;
            }
        }
        private static void RemoveFromList(string key)
        {
            var list = document.DocumentNode.SelectNodes(key);
            if (list != null)
            {
                foreach (HtmlNode test in list)
                    test.Remove();
            }
        }
    }
}
