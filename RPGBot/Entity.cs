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
        [JsonProperty("max_hp")]
        public double MaxHP { get; set; }

        [JsonProperty("class")]
        public long Class { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }
        [JsonProperty("gold")]
        public double Gold { get; set; }

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
            if (isAlive())
            {
                if (BaseSkill.CanBeCasted())
                {
                    SkillManager.Instance.CastBaseSkill(this, targets);
                }
                else
                {
                    double now = DateTime.Now.TimeOfDay.TotalMilliseconds;
                    double exp = BaseSkill.Expiration;
                    double seconds = exp - now;
                    return $"{Name}'s base skill is cooling down: {Utils.FormatSeconds(seconds / 1000)}";
                }
            }
            else
            {
                return $"{Name} you are dead. You will be revived in: {Utils.FormatSeconds(Utils.GetTimeDifference(ReviveTime) / 1000)}";
            }
            return result;
        }
        public bool CanCast(long skill)
        {
            foreach (Skill s in Skills)
                if (s.ID == skill)
                    return true;
            return false;
        }
        public Skill GetSkillByID(long id)
        {
            foreach (Skill s in Skills)
                if (s.ID == id)
                    return s;
            return null;
        }
        public string CastSkill(string name, Entity[] targets)
        {
            string result = "";
            ClassSkill skill = SkillManager.Instance.GetSkillByName(name);
            if (skill == null)
                return "Skill not found";
            else
            {
                if (CanCast(skill.ID))
                {
                    if (isAlive())
                    {
                        Skill s = GetSkillByID(skill.ID);
                        if (s.CanBeCasted())
                        {
                            SkillManager.Instance.CastSkill(s.ID,this, targets);
                        }
                        else
                        {
                            return $"{Name}'s {skill.Name} skill is cooling down: {Utils.FormatSeconds(Utils.GetTimeDifference(s.Expiration) / 1000)}";
                        }
                    }
                    else
                    {
                        return $"{Name} you are dead. You will be revived in: {Utils.FormatSeconds(Utils.GetTimeDifference(ReviveTime) / 1000)}";
                    }
                }
                else
                {
                    return $"{Name} you can not cast {skill.Name}";
                }
            }
            return result;
        }



        public string Die()
        {
            return null;
        }
        public string Revive()
        {
            State = 1;
            ReviveTime = 0;
            HP = MaxHP;
            return $"{Name} has been revived";
        }
        public bool isAlive()
        {
            return State == 1;

        }
        public double CalculateDamage()
        {
            if (this is Player)
                return ClassManager.Instance.GetClassByID(Class).BaseDamage + Stats.Str * 10;
            else
                return ((Mob)this).BaseDamage + this.Stats.Str * 10;
        }
        public double CalculateDefense()
        {
            if (this is Player)
                return ClassManager.Instance.GetClassByID(Class).BaseDefense;
            else
                return ((Mob)this).BaseDefense;
        }
        public double TakeDamage(double damage)
        {
            double newDamage = damage - CalculateDefense();
            if (newDamage < 0)
                newDamage = 0;
            HP -= newDamage;
            return newDamage;
        }

        public void Heal(double amount)
        {
            if(isAlive())
            {
                if (HP < MaxHP)
                    HP += amount;
                if (HP > MaxHP)
                    HP = MaxHP;
            }
        }

        public string ShortDisplayString()
        {
            return "";
        }
        public void Tick()
        {

        }


    }
}
