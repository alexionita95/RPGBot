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

    }
}
