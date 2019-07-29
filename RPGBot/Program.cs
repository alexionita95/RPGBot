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

        
        private Thread worker;




        public async Task MainAsync()
        {
            CommandManager.Instance.LoadCommands();
            Config.Instance.Init();
            ClassManager.Instance.Init();
            SkillManager.Instance.Init();
            MobManager.Instance.Init();
            worker = new Thread(backgroundTask);
            DiscordManager.Instance.Ready = new Action(DiscordReady);
            DiscordManager.Instance.Init();
            await Task.Delay(-1);
        }

        private void DiscordReady()
        {
            worker.Start();
        }
        
        bool running = true;
        private void backgroundTask()
        {
            while (running)
            {
                PlayerManager.Instance.Tick();
                SkillManager.Instance.Tick();
                MobManager.Instance.Tick();
                Thread.Sleep(1000);
            }
        }
    }
}
