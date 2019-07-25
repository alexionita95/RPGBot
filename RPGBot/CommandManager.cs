using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using Discord.WebSocket;

namespace RPGBot
{
    public class CommandManager
    {
        List<Command> commands;
        Commands commandsMethods;

        public CommandManager()
        {
            commands = new List<Command>();
            commandsMethods = new Commands();
        }
        public void LoadCommands()
        {
            using (StreamReader sr = new StreamReader("commands.json"))
            {
                if(sr!=null)
                {
                    string json = sr.ReadToEnd();
                    commands = JsonConvert.DeserializeObject<List<Command>>(json);
                    sr.Close();
                }
            }
        }
        public void SaveCommands()
        {
            string json = JsonConvert.SerializeObject(commands);
            Console.WriteLine(json);
            using (StreamWriter sw = new StreamWriter("commands.json",false))
            {
                sw.Write(json);
                sw.Flush();
                sw.Close();
            }
        }
        public void AddCommand(Command c)
        {
            foreach(string cmd in c.Commands)
            {
                if (CommandExists(cmd))
                    return;

                commands.Add(c);
            }
        }
        public bool CommandExists(string command)
        {
            foreach (Command c in commands)
                if (c.Commands.Contains(command))
                    return true;
            return false;
        }
        public Command FindCommand(string command)
        {
            foreach (Command c in commands)
                if (c.Commands.Contains(command))
                    return c;
            return null;
        }

        public void ExecuteCommand(DiscordSocketClient client, SocketMessage message)
        {
            string[] commandArray = message.Content.Split(' ');
            string command = commandArray[0].Substring(1);
            Command c = FindCommand(command);
            if (c != null)
            {
                MethodInfo method = typeof(Commands).GetMethod(c.Method);
                if (method != null)
                {
                    method.Invoke(commandsMethods,new object[]{client,message});
                }
            }
        }



    }
}
