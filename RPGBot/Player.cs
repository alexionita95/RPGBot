namespace RPGBot
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;

    public partial class Player : Entity
    {
        public new void Tick()
        {
            if (!isAlive())
            {
                if (ReviveTime < DateTime.Now.TimeOfDay.TotalMilliseconds)
                {
                    DiscordManager.Instance.Channel.SendMessageAsync(Utils.GetCodeText(Revive()));

                }
            }
            else
            {
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
        }
    }


    
}
