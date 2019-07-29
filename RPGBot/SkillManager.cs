﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RPGBot
{
    public class SkillManager
    {
        private static SkillManager instance;
        public List<ClassSkill> skills;
        private List<Tuple<long,Action,double>> casting;
        List<Tuple<long, Action, double>> toRemove;
        Skills skillsMethds;
        public static SkillManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SkillManager();

                return instance;
            }
        }

        private SkillManager()
        {
            skillsMethds = new Skills();
            casting = new List<Tuple<long, Action, double>>();
            toRemove = new List<Tuple<long, Action, double>>();
        }
        public void Init()
        {
            LoadSkills();
        }


        public void CastBaseSkill(Entity caster, Entity[] targets)
        {
            ClassSkill skill = GetSkillByID(caster.BaseSkill.ID);
            if(skill!=null)
            {
                MethodInfo method = typeof(Skills).GetMethod(skill.Method);
                if (method != null)
                {
                    Tuple<long, Action, double> cast = new Tuple<long, Action, double>(caster.ID, () =>
                    {
                       caster.BaseSkill.Expiration = DateTime.Now.TimeOfDay.TotalMilliseconds + skill.BaseCooldown*1000; 
                       method.Invoke(skillsMethds, new object[] { caster, targets });
                        if (caster is Player)
                        {
                            if (MobManager.Instance.GetCurrentBoss().HP < 0)
                            {
                                MobManager.Instance.LastBossKiller = caster.ID;
                            }
                        }
                    }, DateTime.Now.TimeOfDay.TotalMilliseconds + skill.CastDuration * 1000);

                    casting.Add(cast);
                }
            }
            else
            {
                if(caster.BaseSkill.ID==-1)
                {
                    MethodInfo method = typeof(Skills).GetMethod("SkillBaseDamage");
                    if (method != null)
                    {
                        Tuple<long, Action, double> cast = new Tuple<long, Action, double>(caster.ID, () =>
                        {
                            caster.BaseSkill.Expiration = DateTime.Now.TimeOfDay.TotalMilliseconds+GetSkillCooldown(-1);
                            method.Invoke(skillsMethds, new object[] { caster, targets });
                            if (caster is Player)
                            {
                                if (MobManager.Instance.GetCurrentBoss().HP < 0)
                                {
                                    MobManager.Instance.LastBossKiller = caster.ID;
                                }
                            }
                        }, DateTime.Now.TimeOfDay.TotalMilliseconds + 1000);

                        casting.Add(cast);
                    }
                }
            }
        }

        public double GetSkillCooldown(long id)
        {
            if (id == -1)
                return 30000;
            else
                return GetSkillByID(id).BaseCooldown * 1000;
        }

        public void CastSkill(int id,Entity caster, Entity[] targets)
        {
            ClassSkill skill = GetSkillByID(id);
            if (skill != null)
            {
                MethodInfo method = typeof(Skills).GetMethod(skill.Method);
                if (method != null)
                {
                    Tuple<long, Action, double> cast = new Tuple<long, Action, double>(caster.ID, () =>
                    {

                        method.Invoke(skillsMethds, new object[] { caster, targets });
                        if (caster is Player)
                        {
                            if (MobManager.Instance.GetCurrentBoss().HP < 0)
                            {
                                MobManager.Instance.LastBossKiller = caster.ID;
                            }
                        }
                    }, DateTime.Now.TimeOfDay.TotalMilliseconds + skill.CastDuration * 1000);

                    casting.Add(cast);
                }
            }
        }


        public ClassSkill GetSkillByID(long id)
        {
            foreach(ClassSkill s in skills)
            {
                if (s.ID == id)
                    return s;
            }
            return null;
        }

        public void Tick()
        {
    
            foreach(Tuple<long,Action,double> cast in casting)
            {
                if(cast.Item3 < DateTime.Now.TimeOfDay.TotalMilliseconds)
                {
                    cast.Item2();
                    toRemove.Add(cast);
                }
            }
            foreach(Tuple<long, Action, double> r in toRemove)
            {
                casting.Remove(r);  
            }
            toRemove.Clear();
        }

        private void LoadSkills ()
        {
            if (File.Exists("skills.json"))
            {
                using (StreamReader sr = new StreamReader("skills.json"))
                {
                    if (sr != null)
                    {
                        string json = sr.ReadToEnd();
                        skills = JsonConvert.DeserializeObject<List<ClassSkill>>(json);
                        sr.Close();
                    }
                    else
                    {
                        skills = new List<ClassSkill>();
                    }
                }
            }
            else
            {
                skills = new List<ClassSkill>();
            }
        }
        public void SaveSkills()
        {
            string json = JsonConvert.SerializeObject(skills);
            Console.WriteLine(json);
            using (StreamWriter sw = new StreamWriter("skills.json", false))
            {
                sw.Write(json);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
