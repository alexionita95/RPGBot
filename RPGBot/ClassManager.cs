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
                    instance = new ClassManager();
                return instance;


            }
        }
        private ClassManager()
        {
        }
        public void Init()
        {
            LoadClasses();
        }
        private void LoadClasses()
        {
            string json = GameManager.Instance.DataManager.GetDataFrom("classes");
            if (json != null)
            {
                classes = JsonConvert.DeserializeObject<List<Class>>(json);
            }
            else
            {
                classes = new List<Class>();
            }
        }
        public Class GetClassByName(string name)
        {
            foreach (Class c in classes)
            {
                if (c.Name.ToLower().Equals(name.ToLower()))
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
        public void SaveClasses()
        {
            string json = JsonConvert.SerializeObject(classes);

            GameManager.Instance.DataManager.SaveDataTo("classes", json);
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
