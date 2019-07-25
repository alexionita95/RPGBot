using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGBot
{
    class Utils
    {
        public static string FormatSeconds(double seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            int h = time.Hours;
            int m = time.Minutes;
            int s = time.Seconds;
            string hs = (h > 0) ? $"{h}h " : "";
            string ms = (m > 0) ? $"{m}m " : "";
            string ss = (s > 0) ? $"{s}s " : "";
            return hs + ms + ss;
        }

        public static string[] GetCommandArgs(string command)
        {
            int index = command.IndexOf(' ');
            if (index < 0)
                return null;
            string[] result = command.Substring(index+1).Split(' ');
            return result;
        }
        public static string GetCodeText(string text)
        {
            return $"```{text}```";
        }

        public static double GetReviveTime(double value)
        {
            return DateTime.Now.TimeOfDay.TotalMilliseconds + value;
        }
    }
}
