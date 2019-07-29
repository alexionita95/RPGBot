using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace RPGBot
{
    class Loot
    {
        [JsonProperty("exp")]
        public double EXP { get; set; }
        [JsonProperty("gold")]
        public double Gold { get; set; }

    }
}
