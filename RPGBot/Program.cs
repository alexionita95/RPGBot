using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.Threading;

namespace RPGBot
{

    class Program
    {
        public static void Main(string[] args)
         => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private Thread worker;

        SocketGuild server;
        SocketTextChannel channel;


        public async Task MainAsync()
        {
            CommandManager.Instance.LoadCommands();
            Config.Instance.Init();
            ClassManager.Instance.Init();
            SkillManager.Instance.Init();
            worker = new Thread(backgroundTask);
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.MessageReceived += MessageReceived;
            _client.Connected += _client_Connected;
            _client.Ready += _client_Ready;

            await _client.LoginAsync(TokenType.Bot, Config.Instance.Token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        SocketGuild GetServer()
        {
            foreach(SocketGuild s in _client.Guilds)
            {
                if (s.Name.Equals(Config.Instance.Server))
                    return s;
            }
            return null;
        }
        SocketTextChannel GetChannel()
        {
            if(server != null)
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
            //worker.Start();
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
        bool running = true;
        private void backgroundTask()
        {
            while (running)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
