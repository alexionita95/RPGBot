using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGBot
{
    public class Commands
    {
        public void HelloCommand(DiscordSocketClient client, SocketMessage message)
        {
            if(message !=null)
                message.Channel.SendMessageAsync("Hello there");
        }

        public void ClassesCommand(DiscordSocketClient client, SocketMessage message)
        {
            if (message != null)
                message.Channel.SendMessageAsync(ClassManager.Instance.ShortDisplayString());
        }

        public void HelpCommand(DiscordSocketClient client, SocketMessage message)
        {

            if (message != null)
            {
                string[] args = Utils.GetCommandArgs(message.Content);
                string response = "";
                if (args==null)
                {
                    response = CommandManager.Instance.GetHelp(null);
                }
                else
                {
                    response = CommandManager.Instance.GetHelp(args[0]);
                }
                message.Channel.SendMessageAsync(response);
            }
        }

        public void CreateCommand(DiscordSocketClient client, SocketMessage message)
        {

            if (message != null)
            {
                string[] args = Utils.GetCommandArgs(message.Content);
                string response;
                if (args != null)
                {
                    response = PlayerManager.Instance.AddPlayer((long)message.Author.Id, message.Author.Username, args[0]);
                }
                else
                {
                    response = "Please specify the class";
                }
                message.Channel.SendMessageAsync(response);
            }
        }
    }
}
