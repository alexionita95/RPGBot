using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace RPGBot
{
    public class FilesInitializer
    {
        public static void InitializeSkills()
        {
            if(!File.Exists("skills.json"))
            {
                ClassSkill s = new ClassSkill();

                SkillManager.Instance.skills.Add(s);
                SkillManager.Instance.SaveSkills();
            }
        }

        public static void InitializeClasses()
        {
            if (!File.Exists("classes.json"))
            {
                ClassManager.Instance.Init();
                Class c = new Class();
                Stats s = new Stats();
                c.Stats = s;
                ClassManager.Instance.classes.Add(c);
                ClassManager.Instance.SaveClasses();
            }
        }

        public static void InitializeBosses()
        {
            MobManager.Instance.Init();
            Mob m = new Mob();
            Skill s = new Skill();
            s.ID = -1;
            m.BaseSkill = s;
            Stats st = new Stats();
            m.Stats = st;
            MobManager.Instance.bosses.Add(m);
            MobManager.Instance.SaveBosses();
        }
        public static void InitializeMinions()
        {

        }
    }
}
