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

        [JsonProperty("loot")]
        public Loot Loot { get; set; }


        public new void Tick()
        {
            if (Utils.GetTimeDifference(BaseSkill.Expiration) < 0)
                BaseSkill.Expiration = 0;

            if(HP < 0)
            {
                Die();
            }
        }

        public new void Die()
        {
            State = 0;
            MobManager.Instance.GetBossByID(ID).Level++;
            PlayerManager.Instance.AddLoot(Loot);
        }

        public new string ShortDisplayString()
        {
            return $"Name: {Name}\nHP: {HP}/{MaxHP}\nLevel:{Level}\nLeaves in:{Utils.FormatSeconds(Utils.GetTimeDifference(LeaveTime) / 1000)}\nTime until next attack:{Utils.FormatSeconds(Utils.GetTimeDifference(BaseSkill.Expiration) / 1000)}";
        }

    }
}
