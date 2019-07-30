using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RPGBot
{
    public partial class Skill
    {
        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }

        [JsonProperty("expiration")]
        public double Expiration { get; set; }

        public bool CanBeCasted()
        {
            return Expiration == 0;
        }

        public string ShortDisplayString()
        {
            ClassSkill s = SkillManager.Instance.GetSkillByID(ID);
            return $"Name: {s.Name}\nCast duration:{Utils.FormatSeconds(s.CastDuration)}\nCooldown:{Utils.FormatSeconds(s.BaseCooldown)}\nTime until next cast: {Utils.FormatSeconds(Utils.GetTimeDifference(Expiration) / 1000)}\n____________________________";
        }
    }

}
