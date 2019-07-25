using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RPGBot
{
    public partial class Stats
    {
        [JsonProperty("vit")]
        public long Vit { get; set; }

        [JsonProperty("str")]
        public long Str { get; set; }

        [JsonProperty("int")]
        public long Int { get; set; }

        [JsonProperty("dex")]
        public long Dex { get; set; }
    }
}
