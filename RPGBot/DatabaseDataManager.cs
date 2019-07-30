using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGBot
{
    public class DatabaseDataManager : IDataManager
    {
        public string GetDataFrom(string name)
        {

            var client = new MongoClient("mongodb+srv://alex:Demo_mec027_Y03@nanohome-rszjd.mongodb.net/test?retryWrites=true&w=majority");
            var database = client.GetDatabase("rpgbot");
            var collection = database.GetCollection<BsonDocument>(name);
            var docs = collection.Find(new BsonDocument()).ToList();
            var doc = docs.First();
            string json = doc.GetElement("values").Value.ToString();
            return json;
        }

        public void SaveDataTo(string name, string data)
        {
            var client = new MongoClient("mongodb+srv://alex:Demo_mec027_Y03@nanohome-rszjd.mongodb.net/test?retryWrites=true&w=majority");
            var database = client.GetDatabase("rpgbot");
            var collection = database.GetCollection<BsonDocument>(name);
            var docs = collection.Find(new BsonDocument()).ToList();
            var doc = docs.First();
            var id = doc.GetElement("_id");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id.Value.ToString()));
            var update = Builders<BsonDocument>.Update.Set("values", data);
            collection.UpdateOne(filter, update);
        }
    }
}
