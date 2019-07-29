namespace RPGBot
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;

    public partial class Player : Entity
    {
        [JsonProperty("exp")]
        public double EXP { get; set; }

        [JsonProperty("needed_exp")]
        public double NeededEXP { get; set; }

        [JsonProperty("available_stat_points")]
        public long AvailableStatPoints { get; set; }


        public new void Tick()
        {
            Heal(Utils.CalculateHeal(this));
            if (!isAlive())
            {
                if (ReviveTime < DateTime.Now.TimeOfDay.TotalMilliseconds && ReviveTime != 0)
                {
                    DiscordManager.Instance.SendMessage(Revive());

                }
            }
            else
            {
                if (HP < 0)
                {
                    DiscordManager.Instance.SendImageMessage(Die(),"https://i.imgur.com/RSbBJEO.gif ");
                }
            }
            if(EXP > NeededEXP)
            {
                EXP -= NeededEXP;
                ++Level;
                ++AvailableStatPoints;
                NeededEXP = Utils.CalculateNeededEXP(Level + 1);
                HP = MaxHP;
                DiscordManager.Instance.SendLevelUpMessage(this);
            }
            foreach (Skill s in Skills)
            {
                if (Utils.GetTimeDifference(s.Expiration) < 0)
                {
                    s.Expiration = 0;
                }
            }
            if (Utils.GetTimeDifference(BaseSkill.Expiration) < 0)
            {
                BaseSkill.Expiration = 0;
            }
        }
        public new string Die()
        {
            State = 0;
            HP = 0;
            ReviveTime = Utils.GetTime(300);
            return $"{Name} died and will be revided in {Utils.FormatSeconds(Utils.GetTimeDifference(ReviveTime) / 1000)}";
        }
        public new string ShortDisplayString()
        {
            return $"Name: {Name}\nHP: {Utils.GetPrettyBar(HP,MaxHP)} ({Math.Floor(HP)}/{MaxHP})\nEXP: {Utils.GetPrettyBar(EXP,NeededEXP)}({EXP}/{NeededEXP})\nLevel:{Level}\nGold:{Gold}";
        }
        public new string StatsString()
        {
            return $"STATS\nName: {Name}\nAvailable Stat Points: {AvailableStatPoints}\nVIT:{Stats.Vit}\nSTR:{Stats.Str}\nINT:{Stats.Int}\nDEX:{Stats.Dex}";
        }
        public new string SkillsString()
        {
            string skillString = "\nClass Skills\n\n";
            for (int i = 0; i < Skills.Count; ++i)
            {
                skillString += $"{Skills[i].ShortDisplayString()}\n";
            }
            return $"SKILLS\nName: {Name}\nBase Skill\n{BaseSkill.ShortDisplayString()}\n{skillString}";
        }
    }


    
}
