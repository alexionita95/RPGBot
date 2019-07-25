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
        private CommandManager commandManager;

        SocketGuild server;
        SocketTextChannel channel;


        public async Task MainAsync()
        {

            commandManager = new CommandManager();
            commandManager.LoadCommands();
            Config.Instance.Init();

            worker = new Thread(backgroundTask);
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.MessageReceived += MessageReceived;
            _client.Connected += _client_Connected;
            _client.Ready += _client_Ready;

            // Remember to keep token private or to read it from an 
            // external source! In this case, we are reading the token 
            // from an environment variable. If you do not know how to set-up
            // environment variables, you may find more information on the 
            // Internet or by using other methods such as reading from 
            // a configuration.
            await _client.LoginAsync(TokenType.Bot, Config.Instance.Token);
            await _client.StartAsync();

            // Block this task until the program is closed.
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
                    commandManager.ExecuteCommand(_client, message);
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
