namespace GreenKartTests.Utils
{
    public static class Consts
    {
        public const string DateTimeFormat = "d.M.yyyy";
        public static bool SaveReportForEachTest { get; internal set; } = false;
        public static bool FixReportTimestamp { get; internal set; } = true;
    }
}
