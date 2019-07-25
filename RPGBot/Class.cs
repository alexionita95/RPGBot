using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RPGBot
{
    public partial class Class
    {
        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("stats")]
        public Stats Stats { get; set; }

        [JsonProperty("skills")]
        public List<ClassSkill> Skills { get; set; }

        [JsonProperty("base_dmg")]
        public long BaseDamage { get; set; }

        [JsonProperty("base_def")]
        public long BaseDeffense { get; set; }
    }
}
