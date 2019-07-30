using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGBot
{
    class FileDataManager : IDataManager
    {
        public string GetDataFrom(string name)
        {
            name = $"{name}.json";
            if (File.Exists(name))
            {
                using (StreamReader sr = new StreamReader(name))
                {
                    if (sr != null)
                    {
                        string json = sr.ReadToEnd();
                        sr.Close();
                        return json;

                    }
                }
            }
            return null;
        }

        public void SaveDataTo(string name, string data)
        {
            name = $"{name}.json";
            using (StreamWriter sw = new StreamWriter(name, false))
            {
                sw.Write(data);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
