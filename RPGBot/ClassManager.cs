using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private static void LoadClasses()
        {
            instance = new ClassManager();
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
            }
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
        public void SaveClasses()
        {
            string json = JsonConvert.SerializeObject(classes);
            Console.WriteLine(json);
            using (StreamWriter sw = new StreamWriter("classes.json", false))
            {
                sw.Write(json);
                sw.Flush();
                sw.Close();
            }
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
