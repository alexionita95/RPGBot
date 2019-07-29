using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGBot
{
    class PlayerManager
    {

        List<Player> players;

        private static PlayerManager instance;
        public static PlayerManager Instance
        {
            get { if (instance == null)
                {
                    instance = new PlayerManager();
                }
                return instance;
            }
        }
        private PlayerManager()
        {
            LoadPlayers();
        }
        public bool PlayerExists(long id)
        {
            foreach(Player p in players)
            {
                if(p.ID == id)
                {
                    return true;
                }
            }
            return false;
        }

        public Player GetPlayerByID(long id)
        {
            foreach (Player p in players)
            {
                if (p.ID == id)
                {
                    return p;
                }
            }
            return null;
        }
        public bool isSomeoneAlive()
        {
            foreach (Player p in players)
                if (p.isAlive())
                    return true;
            return false;
        }
        public string AddPlayer(long id,string name, string cls)
        {
            string result=$"Player {name} was created!";
            Class cl = ClassManager.Instance.GetClassByName(cls);
            if(cl!=null)
            {
                if (!PlayerExists(id))
                {
                    Skill b = new Skill();
                    b.ID = cl.BaseSkill;
                    Stats st = new Stats() { Dex = cl.Stats.Dex, Int = cl.Stats.Int, Str = cl.Stats.Str, Vit = cl.Stats.Vit };
                    List<Skill> sk = new List<Skill>();
                    foreach (long s in cl.Skills)
                    {
                        sk.Add(new Skill() { ID = s, Expiration = 0, Level = 0 });
                    }
                    Player p = new Player() { ID = id, Name = name, BaseSkill = b, Gold = 0, Class = cl.ID, Level = 1, Stats = st, Skills = sk, State = 1};
                    p.MaxHP = Utils.CalculateMaxHP(p);
                    p.HP = p.MaxHP;
                    players.Add(p);
                    SavePlayers();
                }
                else
                {
                    result = $"{name} you already have a character. Use ***me*** command to see more.";
                }
            }
            else
            {
                result = $"Class {cls} does not exist. Use ***classes*** command to see available classes.";
            }
            return result;
        }

        public void Tick()
        {
            foreach(Player p in players)
            {
                p.Tick();
            }
            SavePlayers();
        }

        public Player GetRandomPlayer()
        {
            Random rand = new Random();
            int index = rand.Next(0, players.Count);
            while(!DiscordManager.Instance.IsOnline(players[index].ID))
            {
                index = rand.Next(0, players.Count);
            }
            return players[index];
        }

        public void AddLoot(Loot loot)
        {
            foreach(Player p in players)
            {
                if(p.ID == MobManager.Instance.LastBossKiller)
                {
                    double g = 1.5 * loot.Gold;
                    double e = 1.5 * loot.EXP;
                    p.Gold += g;
                    p.EXP += e;
                    DiscordManager.Instance.SendLootMessage(p.Name, e, g);

                }
                else
                {
                    double g = loot.Gold;
                    double e = loot.EXP;
                    p.Gold += g;
                    p.EXP += e;
                    DiscordManager.Instance.SendLootMessage(p.Name, e, g);
                }
            }
        }

        private void LoadPlayers()
        {
            if (File.Exists("players.json"))
            {
                using (StreamReader sr = new StreamReader("players.json"))
                {
                    if (sr != null)
                    {
                        string json = sr.ReadToEnd();
                        players = JsonConvert.DeserializeObject<List<Player>>(json);
                        sr.Close();
                    }
                    else
                    {
                        players = new List<Player>();
                    }
                }
            }
            else
            {
                players = new List<Player>();
            }
        }
        public void SavePlayers()
        {
            string json = JsonConvert.SerializeObject(players);
            using (StreamWriter sw = new StreamWriter("players.json", false))
            {
                sw.Write(json);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
