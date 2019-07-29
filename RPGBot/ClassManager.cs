using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RPGBot
{
    public class ClassManager
    {
        public List<Class> classes;
        private static ClassManager instance;
        public static ClassManager Instance
        {
            get
            {
                if (instance == null)
                    LoadClasses();
                return instance;


            }
        }
        private ClassManager()
        {

        }
        public void Init()
        {
            if (instance == null)
                LoadClasses();
        }
        private static async void LoadClasses()
        {
            instance = new ClassManager();

            var client = new MongoClient("mongodb+srv://alex:Demo_mec027_Y03@nanohome-rszjd.mongodb.net/test?retryWrites=true&w=majority");
            var database = client.GetDatabase("rpgbot");
            var collection = database.GetCollection<BsonDocument>("classes");
            using (IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> batch = cursor.Current;
                    BsonDocument doc = batch.ElementAt(0);
                    string json = doc.GetElement("values").Value.ToString();
                    instance.classes = JsonConvert.DeserializeObject<List<Class>>(json);

                }
            }
            /*
            if (File.Exists("classes.json"))
            {
                using (StreamReader sr = new StreamReader("classes.json"))
                {
                    if (sr != null)
                    {
                        string json = sr.ReadToEnd();
                        instance.classes = JsonConvert.DeserializeObject<List<Class>>(json);
                        sr.Close();
                    }
                    else
                    {
                        instance.classes = new List<Class>();
                    }
                }
            }
            else
            {
                instance.classes = new List<Class>();
            }*/
        }
        public Class GetClassByName(string name)
        {
            foreach (Class c in classes)
            {
                if(c.Name.ToLower().Equals(name.ToLower()))
                {
                    return c;
                }
            }
            return null;
        }
        public Class GetClassByID(long id)
        {
            foreach (Class c in classes)
            {
                if (c.ID.Equals(id))
                {
                    return c;
                }
            }
            return null;
        }
        public async void SaveClasses()
        {
            string json = JsonConvert.SerializeObject(classes);

            var client = new MongoClient("mongodb+srv://alex:Demo_mec027_Y03@nanohome-rszjd.mongodb.net/test?retryWrites=true&w=majority");
            var database = client.GetDatabase("rpgbot");
            var collection = database.GetCollection<BsonDocument>("classes");
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

            Console.WriteLine(json);/*
            using (StreamWriter sw = new StreamWriter("classes.json", false))
            {
                sw.Write(json);
                sw.Flush();
                sw.Close();
            }*/
        }

        public string ShortDisplayString()
        {
            string result = "";
            for (int i = 0; i < classes.Count; ++i)
                result += classes.ElementAt(i).ShortDisplayString();
            return Utils.GetCodeText(result);
        }
    }
}
