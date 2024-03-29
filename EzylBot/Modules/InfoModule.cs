﻿using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Discord.WebSocket;

namespace EzylBot.Modules
{
    public class InfoModule: ModuleBase<SocketCommandContext>
    {
        public CommandService _command;
        public InfoModule(CommandService command)
        {
            _command = command;
        }

        private readonly Color _color = new Color(111, 38, 184);

        [Command("help")]
        [Summary("Get a list of all command or, if you give a command name after help\n" +
            "like : help help, \n" +
            "this will give you a summary of the gived command name.")]
        public async Task HelpCommand(string commandtest = null)
        {
            List<CommandInfo> commands = _command.Commands.ToList();
            EmbedBuilder embedBuilder = new EmbedBuilder()
                    .WithCurrentTimestamp()
                    .WithColor(_color);
            if (commandtest == null)
            {
                embedBuilder.WithDescription("Here's the commands :");
                string infomodule = "";
                string musicmodule = "";
                string adminmodule = "";
                string funmodule = "";
                string reactmodule = "";
                for (int a = 0; a < commands.Count; a++)
                {
                    CommandInfo command = commands[a];
                    switch (command.Module.Name)
                    {
                        case "InfoModule":
                            infomodule = infomodule + command.Name + ", ";
                            break;
                        case "music":
                            musicmodule = musicmodule + command.Name + ", ";
                            break;
                        case "Admin":
                            adminmodule = adminmodule + command.Name + ", ";
                            break;
                        case "Fun":
                            funmodule = funmodule + command.Name + ", ";
                            break;
                        case "React":
                            reactmodule = reactmodule + command.Name + ", ";
                            break;
                        default:
                            break;
                    }
                }
                infomodule = infomodule.Remove(infomodule.Length - 2);
                musicmodule = musicmodule.Remove(musicmodule.Length - 2);
                adminmodule = adminmodule.Remove(adminmodule.Length - 2);
                funmodule = funmodule.Remove(funmodule.Length - 2);
                embedBuilder.AddField("Administration :", adminmodule);
                embedBuilder.AddField("Info :", infomodule);
                embedBuilder.AddField("Fun commands :", funmodule);
                embedBuilder.AddField("Music (In dev) :", musicmodule);
                embedBuilder.AddField("Gif stuff :", reactmodule);
            }
            else
            {
                bool find = false;
                int i = 0;
                while (i < commands.Count && find==false)
                {
                    CommandInfo command = commands[i];
                    if (command.Name.Equals(commandtest))
                    {
                        embedBuilder.AddField(command.Name, command.Summary ?? "No description available\n");
                        find = true;
                    }
                    i++;
                }
                if (find == false)
                {
                    embedBuilder.AddField("Command not found", "Fuck you, the command don't exist");
                    embedBuilder.WithImageUrl("https://cdn.discordapp.com/attachments/759864193586954262/847457684353318912/158943914_1168678140218365_7930704539137601834_n.jpg");
                }
            }
            EmbedAuthorBuilder author = new EmbedAuthorBuilder()
                .WithName(Context.Client.CurrentUser.Username)
                .WithIconUrl(Context.Client.CurrentUser.GetAvatarUrl());
            embedBuilder.WithAuthor(author);
            Embed embed = embedBuilder.Build();
            await ReplyAsync(embed: embed);
        }

        [Command("ping")]
        [Summary("Get the latency of the bot.")]
        public async Task Ping()
        {
            var EmbedBuilder = new EmbedBuilder()
                .WithDescription($"Pong! :ping_pong: \n {Context.Client.Latency}**ms**")
                .WithColor(_color);
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }

        [Command("avatar")]
        [Summary("Give link to the discord avatar")]
        public async Task Avatar(IGuildUser user = null)
        {
            EmbedBuilder embedBuilder = new EmbedBuilder().WithCurrentTimestamp();
            if (user == null)
            {
                string url = Context.User.GetAvatarUrl(ImageFormat.Auto, 4096);
                string name = Context.User.Username;
                embedBuilder.WithTitle(name)
                    .WithDescription($"[Lien direct]({url})")
                    .WithImageUrl(url);
            }
            else
            {
                string url = user.GetAvatarUrl(ImageFormat.Auto, 4096);
                string name = user.Username;
                embedBuilder.WithTitle(name)
                    .WithDescription($"[Lien direct]({url})")
                    .WithImageUrl(url);
            }
            Embed embed = embedBuilder.Build();
            await ReplyAsync(embed: embed);
        }
    }
}
