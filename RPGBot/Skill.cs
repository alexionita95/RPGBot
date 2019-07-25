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
        public long Expiration { get; set; }
    }

}
