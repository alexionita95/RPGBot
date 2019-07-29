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
            Console.WriteLine($"{caster.Name} Casted Capac");
            DiscordManager.Instance.Channel.SendMessageAsync($"{caster.Name} Casted Capac");
        }

        public void SkillBaseDamage(Entity caster, Entity[] targets)
        {
            Console.WriteLine($"{caster.Name} Casted Base Damage");
            DiscordManager.Instance.Channel.SendMessageAsync($"{caster.Name} Casted Base Damage");
        }
    }
}
