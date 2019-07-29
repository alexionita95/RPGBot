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
            if (seconds < 0)
                return "now";
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            int h = time.Hours;
            int m = time.Minutes;
            int s = time.Seconds;
            string hs = (h > 0) ? $"{h}h " : "";
            string ms = (m > 0) ? $"{m}m " : "";
            string ss = (s > 0) ? $"{s}s " : "";
            return hs + ms + ss;
        }
        public static double GetTimeDifference(double value)
        {
            return value - DateTime.Now.TimeOfDay.TotalMilliseconds;
        }

        public static string[] GetCommandArgs(string command)
        {
            int index = command.IndexOf(' ');
            if (index < 0)
                return null;
            string[] result = command.Substring(index + 1).Split(' ');
            return result;
        }
        public static string GetCodeText(string text)
        {
            return $"```{text}```";
        }

        public static double GetReviveTime(double value)
        {
            return DateTime.Now.TimeOfDay.TotalMilliseconds + value*1000;
        }
        public static double CalculateNeededEXP(long level)
        {
            return 100 + Math.Pow(2, level);
        }

        public static double CalculateMaxHP(Entity entity)
        {
            if(entity is Mob)
            {
                return ((Mob)entity).BaseHP + entity.Level*entity.Stats.Vit * 100;
            }
            if(entity is Player)
            {
                return ClassManager.Instance.GetClassByID(entity.Class).BaseHP + entity.Stats.Vit * 100;
            }
            return 0;
        }

        public static double CalculateDamage(Entity entity)
        {
            if (entity is Mob)
            {
                return ((Mob)entity).BaseDamage + entity.Level*entity.Stats.Str * 5;
            }
            if (entity is Player)
            {
                return ClassManager.Instance.GetClassByID(entity.Class).BaseDamage + entity.Stats.Str * 5;
            }
            return 0;
        }

        public static double CalculateHeal(Entity entity)
        {
           /* if (entity is Mob)
            {
                return ((Mob)entity).BaseDamage + entity.Level * entity.Stats.Str * 5;
            }*/
            if (entity is Player)
            {
                return entity.Stats.Vit*2;
            }
            return 0;
        }



    }
}
