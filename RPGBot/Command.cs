namespace RPGBot
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Command
    {
        [JsonProperty("commands")]
        public List<string> Commands { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        public Command()
        {
            Commands = new List<string>();
        }

        public string DisplayString()
        {
            string cmds = "[ ";
            for (int i = 0; i < Commands.Count - 1; ++i)
            {
                cmds += Commands[i] + ", ";
            }
            cmds += (Commands[Commands.Count - 1] + "]");

            return $"```Command Aliases: {cmds}\nDescription:{Description}\n```";
        }
        public string DisplayShortString()
        {

            return $"{Commands[0]}";
        }

    }
}
