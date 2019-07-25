using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RPGBot
{
    public class Entity
    {
        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("class")]
        public long Class { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }
        [JsonProperty("gold")]
        public long Gold { get; set; }

        [JsonProperty("stats")]
        public Stats Stats { get; set; }

        [JsonProperty("base_skill")]
        public Skill BaseSkill { get; set; }

        [JsonProperty("skills")]
        public List<Skill> Skills { get; set; }

        [JsonProperty("state")]
        public long State { get; set; }

        [JsonProperty("revive_time")]
        public double ReviveTime { get; set; }

        public void Die()
        {
            State = 0;
            ReviveTime = Utils.GetReviveTime(300);
        }
        public void Revive()
        {
            State = 1;
            ReviveTime = 0;
        }
        public bool isAlive()
        {
            return State == 1;

        }
        public void Tick()
        {
            if(!isAlive())
            {
                if(ReviveTime < DateTime.Now.TimeOfDay.TotalMilliseconds)
                {
                    Revive();
                }
            }
        }

    }
}
