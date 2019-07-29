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

        public new void Tick()
        {
            Heal(Utils.CalculateHeal(this));
            if (!isAlive())
            {
                if (ReviveTime < DateTime.Now.TimeOfDay.TotalMilliseconds && ReviveTime != 0)
                {
                    DiscordManager.Instance.Channel.SendMessageAsync(Utils.GetCodeText(Revive()));

                }
            }
            else
            {
                if (HP < 0)
                    DiscordManager.Instance.Channel.SendMessageAsync(Utils.GetCodeText(Die()));
            }
            if(EXP > NeededEXP)
            {
                EXP -= NeededEXP;
                ++Level;
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
            ReviveTime = Utils.GetReviveTime(300);
            return $"{Name} died and will be revided in {Utils.FormatSeconds(Utils.GetTimeDifference(ReviveTime) / 1000)}";
        }
        public new string ShortDisplayString()
        {
            return $"Name: {Name}\nHP: {HP}/{MaxHP}\nEXP:{EXP}/{NeededEXP}\nLevel:{Level}\nGold:{Gold}\nTime until next attack:{Utils.FormatSeconds(Utils.GetTimeDifference(BaseSkill.Expiration) / 1000)}";
        }
    }


    
}
