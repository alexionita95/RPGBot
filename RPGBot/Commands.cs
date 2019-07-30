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
            if (message != null)
                message.Channel.SendMessageAsync("Hello there");
        }

        public void ClassesCommand(DiscordSocketClient client, SocketMessage message)
        {
            if (message != null)
                DiscordManager.Instance.SendMessage(ClassManager.Instance.ShortDisplayString());
        }

        public void HelpCommand(DiscordSocketClient client, SocketMessage message)
        {

            if (message != null)
            {
                string[] args = Utils.GetCommandArgs(message.Content);
                string response = "";
                if (args == null)
                {
                    response = CommandManager.Instance.GetHelp(null);
                }
                else
                {
                    response = CommandManager.Instance.GetHelp(args[0]);
                }
                DiscordManager.Instance.SendMessage(response);
            }
        }

        public void AttackCommand(DiscordSocketClient client, SocketMessage message)
        {

            if (message != null)
            {
                string[] args = Utils.GetCommandArgs(message.Content);
                string response = "";
                if (args == null)
                {
                    ulong id = message.Author.Id;
                    if (PlayerManager.Instance.PlayerExists((long)id))
                    {
                        Player p = PlayerManager.Instance.GetPlayerByID((long)id);
                        Mob boss = MobManager.Instance.GetCurrentBoss();
                        if (boss != null)
                            DiscordManager.Instance.SendMessage(p.CastBaseSkill(new Entity[] { boss }));
                        else
                        {
                            DiscordManager.Instance.SendMessage(MobManager.Instance.DisplayCurrentBoss());
                        }
                    }
                    else
                    {
                        DiscordManager.Instance.SendMessage($"{message.Author.Username} you do not have a character. Use ***create*** command to create one.");
                    }
                }
                DiscordManager.Instance.SendMessage(response);
            }
        }

        public void CastCommand(DiscordSocketClient client, SocketMessage message)
        {

            if (message != null)
            {
                string[] args = Utils.GetCommandArgs(message.Content);
                string response = "";
                if (args == null)
                {
                    response = "Use help to see command usage";
                }
                else
                {
                    ulong id = message.Author.Id;
                    if (PlayerManager.Instance.PlayerExists((long)id))
                    {
                        Player p = PlayerManager.Instance.GetPlayerByID((long)id);
                        DiscordManager.Instance.SendMessage(p.CastSkill(args[0], null));
                    }
                }
                DiscordManager.Instance.SendMessage(response);
            }
        }

        public void MeCommand(DiscordSocketClient client, SocketMessage message)
        {

            if (message != null)
            {
                string[] args = Utils.GetCommandArgs(message.Content);
                string response = "";
                if (args == null)
                {
                    ulong id = message.Author.Id;
                    if (PlayerManager.Instance.PlayerExists((long)id))
                    {
                        Player p = PlayerManager.Instance.GetPlayerByID((long)id);
                        DiscordManager.Instance.SendMessage(p.ShortDisplayString());
                    }
                    else
                    {
                        DiscordManager.Instance.SendMessage($"{message.Author.Username} you do not have a character. Use ***create*** command to create one.");
                    }
                }
                else
                {
                    Player p = null;
                    ulong id = message.Author.Id;
                    if (PlayerManager.Instance.PlayerExists((long)id))
                    {
                        p = PlayerManager.Instance.GetPlayerByID((long)id);
                    }
                    else
                    {
                        p = null;
                        DiscordManager.Instance.SendMessage($"{message.Author.Username} you do not have a character. Use ***create*** command to create one.");
                    }
                    if (p != null)
                    {
                        switch (args[0].ToLower())
                        {
                            case "stats":
                                {

                                    DiscordManager.Instance.SendMessage(p.StatsString());
                                }
                                break;
                            case "skills":
                                {
                                    DiscordManager.Instance.SendMessage(p.SkillsString());
                                }
                                break;
                            case "attributes":
                                {
                                    DiscordManager.Instance.SendMessage(p.AttributesString());
                                }
                                break;
                        }
                    }
                }
                DiscordManager.Instance.SendMessage(response);
            }
        }

        public void BossCommand(DiscordSocketClient client, SocketMessage message)
        {

            if (message != null)
            {
                DiscordManager.Instance.SendMessage(MobManager.Instance.DisplayCurrentBoss());
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
