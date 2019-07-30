namespace RPGBot
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using System.IO;

    public partial class Config
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }
        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        private static Config instance;
        public static Config Instance
        {
            get
            {
                if (instance == null)
                    LoadConfig();
                return instance;
            }
        }
        private Config()
        {

        }
        public void Init()
        {
            if (instance == null)
                LoadConfig();
        }
        private static void LoadConfig()
        {
            if (File.Exists("config.json"))
            {
                using (StreamReader sr = new StreamReader("config.json"))
                {
                    if (sr != null)
                    {
                        string json = sr.ReadToEnd();
                        instance = JsonConvert.DeserializeObject<Config>(json);
                        sr.Close();
                    }
                    else
                    {
                        instance = new Config();
                    }
                }
            }
            else
            {
                instance = new Config();
            }
        }
        public static void SaveConfig()
        {
            string json = JsonConvert.SerializeObject(instance);
            Console.WriteLine(json);
            using (StreamWriter sw = new StreamWriter("config.json", false))
            {
                sw.Write(json);
                sw.Flush();
                sw.Close();
            }
        }
    }
}