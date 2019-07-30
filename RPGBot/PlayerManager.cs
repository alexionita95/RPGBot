using MongoDB.Bson;
using MongoDB.Driver;
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
            get
            {
                if (instance == null)
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
            foreach (Player p in players)
            {
                if (p.ID == id)
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
        public string AddPlayer(long id, string name, string cls)
        {
            string result = $"Player {name} was created!";
            Class cl = ClassManager.Instance.GetClassByName(cls);
            if (cl != null)
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
                    Player p = new Player() { ID = id, Name = name, BaseSkill = b, Gold = 0, Class = cl.ID, Level = 1, Stats = st, Skills = sk, State = 1 };
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
            foreach (Player p in players)
            {
                p.Tick();
            }
            SavePlayers();
        }

        public Player GetRandomPlayer()
        {
            Random rand = new Random();
            int index = rand.Next(0, players.Count);
            while (!DiscordManager.Instance.IsOnline(players[index].ID))
            {
                index = rand.Next(0, players.Count);
            }
            return players[index];
        }

        public void AddLoot(Loot loot)
        {
            List<string> message = new List<string>();
            foreach (Player p in players)
            {

                if (p.ID == MobManager.Instance.LastBossKiller)
                {
                    double g = 1.5 * loot.Gold;
                    double e = 1.5 * loot.EXP;
                    p.Gold += g;
                    p.EXP += e;
                    message.Add($"{p.Name} received {e} EXP and {g} Gold");

                }
                else
                {
                    double g = loot.Gold;
                    double e = loot.EXP;
                    p.Gold += g;
                    p.EXP += e;
                    message.Add($"{p.Name} received {e} EXP and {g} Gold");
                }
            }
            DiscordManager.Instance.SendCombinedMessage(message.ToArray());
        }


        private void LoadPlayers()
        {
            string json = GameManager.Instance.DataManager.GetDataFrom("players");

            if (json != null)
            {
                players = JsonConvert.DeserializeObject<List<Player>>(json);
                if (players == null)
                {
                    players = new List<Player>();
                }
            }
            else
            {
                players = new List<Player>();
            }
        }
        public async void SavePlayers()
        {
            if (players != null)
            {
                string json = JsonConvert.SerializeObject(players, Formatting.Indented);
                if (json != null)
                {

                    var client = new MongoClient("mongodb+srv://alex:Demo_mec027_Y03@nanohome-rszjd.mongodb.net/test?retryWrites=true&w=majority");
                    var database = client.GetDatabase("rpgbot");
                    var collection = database.GetCollection<BsonDocument>("players");
                    using (IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(new BsonDocument()))
                    {
                        while (await cursor.MoveNextAsync())
                        {
                            IEnumerable<BsonDocument> batch = cursor.Current;
                            BsonDocument doc = batch.ElementAt(0);
                            doc.Set("values", json);
                            var id = doc.GetElement("_id");
                            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id.Value.ToString()));
                            var update = Builders<BsonDocument>.Update.Set("values", json);
                            collection.UpdateOne(filter, update);

                        }
                    }

                    GameManager.Instance.DataManager.SaveDataTo("players", json);
                }
            }
        }
    }
}
