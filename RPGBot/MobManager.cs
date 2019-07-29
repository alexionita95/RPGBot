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

        long lastBossKiller;
        public long LastBossKiller
        {
            get { return lastBossKiller; }
            set { lastBossKiller = value; }
        }

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

        public Mob GetCurrentBoss()
        {
            if(currentBoss !=null)
            {
                if (currentBoss.isAlive())
                    return currentBoss;
            }
            return null;
        }
        public Mob GetBossByID(long id)
        {
            foreach(Mob m in bosses)
            {
                if (m.ID == id)
                    return m;
            }
            return null;
        }
        private string DisplayTimeUntilNextBoss()
        {
            return $"No boss available!\nTime until next boss: {Utils.FormatSeconds(Utils.GetTimeDifference(timeUntilNextBoss) / 1000)}";
        }
        public string DisplayCurrentBoss()
        {
            if(currentBoss== null)
            {
                return DisplayTimeUntilNextBoss();
            }
            return currentBoss.ShortDisplayString();
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
                DiscordManager.Instance.SendMessage(DisplayTimeUntilNextBoss());
            }
        }
        private void SpawnBoss()
        {
            Mob boss = bosses.ElementAt(currentBossIndex);
            currentBoss = new Mob()
            {
                Name = boss.Name,
                BaseSkill = boss.BaseSkill,
                Stats = new Stats()
                {
                    Dex = boss.Stats.Dex,
                    Int = boss.Stats.Int,
                    Vit = boss.Stats.Vit,
                    Str = boss.Stats.Str
                },
                BaseHP = boss.BaseHP,
                BaseDamage = boss.BaseDamage,
                BaseDefense = boss.BaseDefense,
                LeaveTime = DateTime.Now.TimeOfDay.TotalMilliseconds + boss.LeaveTime*1000,
                Level = boss.Level,
                State = 1,
                Loot = new Loot() {Gold=boss.Loot.Gold*boss.Level, EXP=boss.Loot.EXP*boss.Level }
            };
            currentBoss.MaxHP = Utils.CalculateMaxHP(currentBoss);
            currentBoss.HP = currentBoss.MaxHP;
            currentBoss.BaseDamage = Utils.CalculateDamage(currentBoss);
            currentBoss.BaseSkill.Expiration = DateTime.Now.TimeOfDay.TotalMilliseconds + SkillManager.Instance.GetSkillCooldown(currentBoss.BaseSkill.ID);
            DiscordManager.Instance.SendMessage($"New Boss spawned!\n{DisplayCurrentBoss()}");
        }

        public bool IsBossAlive()
        {
            return currentBoss != null;
        }
        void BossAttack()
        {
            if(currentBoss!=null)
            {
                currentBoss.Tick();
                if (currentBoss.isAlive())
                {
                    if (PlayerManager.Instance.isSomeoneAlive())
                    {
                        if (currentBoss.BaseSkill.CanBeCasted())
                        {
                            Player p = PlayerManager.Instance.GetRandomPlayer();
                            while (!p.isAlive())
                            {
                                p = PlayerManager.Instance.GetRandomPlayer();
                            }
                            currentBoss.CastBaseSkill(new Entity[] { p });
                        }
                    }
                }
                else
                {
                    DiscordManager.Instance.SendMessage($"{currentBoss.Name} was killed by {PlayerManager.Instance.GetPlayerByID(lastBossKiller).Name}");
                    lastBossKiller = 0;
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
                    else
                    {
                        if(Utils.GetTimeDifference(currentBoss.LeaveTime)<0)
                        {
                            DiscordManager.Instance.SendMessage($"{currentBoss.Name} left");
                            DiscordManager.Instance.SendNormalMessage("https://i.imgur.com/p06VNJM.png ");
                            currentBoss = null;
                        }
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
            BossAttack();
            CheckBoss();
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
            string json = JsonConvert.SerializeObject(bosses, Formatting.Indented);
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
