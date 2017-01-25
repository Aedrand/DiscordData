﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordData
{
    class DiscordDataBot
    {
        DiscordClient client;
        CommandService commands;

        public DiscordDataBot()
        {
            client = new DiscordClient(input =>
            {
                input.LogLevel = LogSeverity.Info;
                input.LogHandler = Log;
            });

            client.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });

            commands = client.GetService<CommandService>();

            client.ExecuteAndWait(async () =>
            {
                await client.Connect("MjMwMDcxNTM5OTE4MzA3MzM5.C2mCFA.0Y4Lu7HQXCz6PCYVxYL_ZYuSKuk", TokenType.Bot);
            });
            
        }

        public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        public void setupCommands()
        {
            commands.CreateCommand("add").Do(async (e) =>
            {
                await e.Channel.SendMessage("Did it.");
            });

            commands.CreateCommand("show")
            .Parameter("Username", ParameterType.Required)
            .Do(async (e) =>
            {
                await e.Channel.SendMessage("lmao");
            });
        }
    }
}