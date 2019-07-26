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
    }
}
