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
    }
}
