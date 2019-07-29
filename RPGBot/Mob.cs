using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGBot
{
    class Mob : Entity
    {
        [JsonProperty("minions")]
        public List<long> Minions { get; set; }

        [JsonProperty("leave_time")]
        public double LeaveTime { get; set; }

        [JsonProperty("spawn_time")]
        public double SpawnTime { get; set; }
        [JsonProperty("base_dmg")]
        public double BaseDamage { get; set; }
        [JsonProperty("base_def")]
        public double BaseDefense { get; set; }
        [JsonProperty("base_hp")]
        public double BaseHP { get; set; }

        public new void Tick()
        {

        }

    }
}
