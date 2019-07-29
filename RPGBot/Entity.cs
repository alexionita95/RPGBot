using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RPGBot
{
    public partial class Entity
    {
        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("hp")]
        public double HP { get; set; }

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
        public string CastBaseSkill(Entity[] targets)
        {
            string result = "";
            if(BaseSkill.CanBeCasted())
            {
                SkillManager.Instance.CastBaseSkill(this, targets);
            }
            else
            {
                double now = DateTime.Now.TimeOfDay.TotalMilliseconds;
                double exp = BaseSkill.Expiration;
                double seconds = exp - now;
                return $"{Name}'s base skill is cooling down: {Utils.FormatSeconds(seconds/1000)}";
            }
            return result;
        }
        public string Die()
        {
            State = 0;
            ReviveTime = Utils.GetReviveTime(300);
            return $"{Name} died";
        }
        public string Revive()
        {
            State = 1;
            ReviveTime = 0;
            return $"{Name} has been revived";
        }
        public bool isAlive()
        {
            return State == 1;

        }
        public void Tick()
        {

        }


    }
}
