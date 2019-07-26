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

        public DiscordSocketClient Client
        {
            get { return _client; }
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

        private Task _client_Ready()
        {
            server = GetServer();
            if (server != null)
            {
                channel = GetChannel();
            }
            channel.SendMessageAsync("RPG Bot ready");
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
