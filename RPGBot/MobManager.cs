using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGBot
{
    class MobManager
    {
        public List<Mob> bosses;
        public List<Mob> minions;
        int currentBossIndex;
        Mob currentBoss;
        List<Mob> currentMinions;
        double timeUntilNextBoss;
        private static MobManager instance;
        public static MobManager Instance
        {
            get {
                if(instance == null)
                {
                    instance = new MobManager();
                }
                return instance; }
        }
        private MobManager()
        {

        }
        public void Init()
        {
            LoadBosses();
            LoadMinions();

        }
        private void PrepareBossSpawn()
        {
            Random rand = new Random();
            int index = rand.Next(bosses.Count - 1);
            Mob boss = bosses.ElementAt(index);
            if (boss != null)
            {
                currentBossIndex = index;
                timeUntilNextBoss = DateTime.Now.TimeOfDay.TotalMilliseconds + boss.SpawnTime * 1000;
                DiscordManager.Instance.Channel.SendMessageAsync($"Time until next boss: {Utils.FormatSeconds(Utils.GetTimeDifference(timeUntilNextBoss)/1000)}");
            }
        }
        private void SpawnBoss()
        {
            Mob boss = bosses.ElementAt(currentBossIndex);
            currentBoss = new Mob() { Name = boss.Name, BaseSkill = boss.BaseSkill, Stats = new Stats() {
                Dex = boss.Stats.Dex,
                Int = boss.Stats.Int,
                Vit = boss.Stats.Vit,
                Str=boss.Stats.Str
            },BaseDamage=boss.BaseDamage,BaseDefense=boss.BaseDefense,LeaveTime=boss.LeaveTime, State=1};
            currentBoss.HP = currentBoss.BaseHP + 10 * currentBoss.Stats.Vit;
            currentBoss.BaseDamage = currentBoss.BaseDamage + 5 * currentBoss.Stats.Str;
        }
        void BossAttack()
        {
            if(currentBoss!=null)
            {
                if(currentBoss.BaseSkill.CanBeCasted())
                {
                    currentBoss.CastBaseSkill(null);
                }
            }
        }
        private void CheckBoss()
        {
            if (timeUntilNextBoss == 0)
            {
                if (currentBoss == null)
                {
                    PrepareBossSpawn();
                }
                else
                {
                    if (!currentBoss.isAlive())
                    {
                        currentBoss = null;
                    }
                }
            }
            else
            {
                if(Utils.GetTimeDifference(timeUntilNextBoss)<0)
                {
                    timeUntilNextBoss = 0;
                    SpawnBoss();
                }
            }
        }
        public void Tick()
        {
            CheckBoss();
            BossAttack();
        }
        private void LoadBosses()
        {
            if (File.Exists("bosses.json"))
            {
                using (StreamReader sr = new StreamReader("bosses.json"))
                {
                    if (sr != null)
                    {
                        string json = sr.ReadToEnd();
                        bosses = JsonConvert.DeserializeObject<List<Mob>>(json);
                        sr.Close();
                    }
                    else
                    {
                        bosses = new List<Mob>();
                    }
                }
            }
            else
            {
                bosses = new List<Mob>();
            }
        }
        public void SaveBosses()
        {
            string json = JsonConvert.SerializeObject(bosses);
            Console.WriteLine(json);
            using (StreamWriter sw = new StreamWriter("bosses.json", false))
            {
                sw.Write(json);
                sw.Flush();
                sw.Close();
            }
        }

        private void LoadMinions()
        {
            if (File.Exists("minions.json"))
            {
                using (StreamReader sr = new StreamReader("minions.json"))
                {
                    if (sr != null)
                    {
                        string json = sr.ReadToEnd();
                        minions = JsonConvert.DeserializeObject<List<Mob>>(json);
                        sr.Close();
                    }
                    else
                    {
                        minions = new List<Mob>();
                    }
                }
            }
            else
            {
                minions = new List<Mob>();
            }
        }
        public void SaveMinions()
        {
            string json = JsonConvert.SerializeObject(minions);
            Console.WriteLine(json);
            using (StreamWriter sw = new StreamWriter("minions.json", false))
            {
                sw.Write(json);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
