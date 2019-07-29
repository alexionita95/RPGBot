using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGBot
{
    public class Skills
    {
        public void SkillCapac(Entity caster, Entity[] targets)
        {
            Console.WriteLine();

            if (targets != null)
            {
                foreach (Entity e in targets)
                {
                    double damage = 2 * caster.CalculateDamage();
                    e.TakeDamage(caster, SkillManager.Instance.GetSkillByID(caster.BaseSkill.ID).Name, 2);
                }
            }
            
        }
        public void SkillParizer(Entity caster, Entity[] targets, ClassSkill skill)
        {
            double hp = 200;
            caster.Heal(hp);
            DiscordManager.Instance.SendMessage($"{caster.Name} casted {skill.Name} and was healed by {hp} HP (HP: {caster.HP}) ");

        }

        public void SkillBaseDamage(Entity caster, Entity[] targets)
        {
            Console.WriteLine($"{caster.Name} Casted Base Damage");
            if (targets != null)
            {
                foreach(Entity e in targets)
                {
                    double damage = caster.CalculateDamage();
                    damage = e.TakeDamage(caster,"Base Damage",1);
                }
            }
            
        }
    }
}
