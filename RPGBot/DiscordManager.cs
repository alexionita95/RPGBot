using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGBot
{
    public class DiscordManager
    {

        private SocketGuild server;
        private SocketTextChannel channel;
        private DiscordSocketClient _client;
        private Action ready;
        public DiscordSocketClient Client
        {
            get { return _client; }
        }
        public Action Ready
        {
            get { return ready; }
            set { ready = value; }
        }

        public SocketGuild Server
        {
            get { return server; }
        }
        public SocketTextChannel Channel
        {
            get { return channel; }
        }
        private static DiscordManager instance;
        public static DiscordManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new DiscordManager();
                }
                return instance;
            }
        }
        private DiscordManager()
        {

        }
        public async void Init()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.MessageReceived += MessageReceived;
            _client.Connected += _client_Connected;
            _client.Ready += _client_Ready;

            await _client.LoginAsync(TokenType.Bot, Config.Instance.Token);
            await _client.StartAsync();
        }

        SocketGuild GetServer()
        {
            foreach (SocketGuild s in _client.Guilds)
            {
                if (s.Name.Equals(Config.Instance.Server))
                    return s;
            }
            return null;
        }
        SocketTextChannel GetChannel()
        {
            if (server != null)
            {
                foreach (SocketTextChannel c in server.TextChannels)
                    if (c.Name.Equals(Config.Instance.Channel))
                        return c;
            }
            return null;
        }

        public bool IsOnline(long id)
        {
            foreach (SocketGuildUser u in server.Users)
                if (u.Id==(ulong)id && u.Status == UserStatus.Online)
                    return true;
            return false;
        }

        public void SendCastMessage(Entity caster, Entity target, string skillName, double damage)
        {
            SendMessage($"{caster.Name} casted {skillName} on {target.Name} (-{damage} HP). Remaning HP: {target.HP}");
        }

        public void SendLevelUpMessage(Player player)
        {
            SendMessage($"{player.Name} leveled up and is now level {player.Level}");
        }

        public void SendLootMessage(string name, double exp, double gold)
        {
            SendMessage($"{name} received {exp} EXP and {gold} Gold");
        }

        public void SendMessage(string message)
        {
            if(message.Length != 0)
            Channel.SendMessageAsync(Utils.GetCodeText(message));
        }

        public void SendCombinedMessage(string[] message)
        {
            if (message.Length != 0)
            {
                string full = "";
                foreach(string m in message)
                {
                    if (message.Length != 0)
                    {
                        full+=m+"\n";
                    }
                }
                Channel.SendMessageAsync(Utils.GetCodeText(full));
            }
        }
        public void SendImageMessage(string message, string image)
        {
            string full = Utils.GetCodeText(message);
            full += "\n" + image;
            Channel.SendMessageAsync(full);
        }

        public void SendNormalMessage(string message)
        {
            if (message.Length != 0)
                Channel.SendMessageAsync(message);
        }

        private Task _client_Ready()
        {
            server = GetServer();
            if (server != null)
            {
                channel = GetChannel();
            }
            SendMessage("RPG Bot ready");
            if(ready!=null)
            {
                ready();
            }
            return Task.CompletedTask;
        }

        private Task _client_Connected()
        {

            return Task.CompletedTask;
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Channel == channel)
            {
                if (message.Content[0].ToString() == Config.Instance.Prefix)
                {
                    CommandManager.Instance.ExecuteCommand(_client, message);
                }
            }
        }
    }
}
