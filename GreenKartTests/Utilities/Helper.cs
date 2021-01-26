using System;
using System.Threading;

namespace GreenKartTests.Utils
{
    public static class Helper
    {
        internal static string Guid(int length = 7)
        {
            return System.Guid.NewGuid().ToString().Substring(0, length);
        }
        
        public static void Wait(int value)
        {            
            Thread.Sleep(400 * value);            
        }

        internal static string Humanize(string methodName)
        {
            return methodName.Replace('_', ' ');
        }

        public static string ToUpperFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            char[] letters = source.ToLower().ToCharArray();
            letters[0] = char.ToUpper(letters[0]);
            return new string(letters);
        }

        internal static string AddArgs(params string[] args)
        {   
            return " (" + String.Join(",", args) + ")";            
        }

        public static string FormatDate(DateTime datetime)
        {
            return datetime.ToString(Consts.DateTimeFormat);
        }

    }
}
