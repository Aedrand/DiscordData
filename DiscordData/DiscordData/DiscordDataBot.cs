using System;
using System.Collections.Generic;
using Discord;
using Discord.Commands;
using System.Diagnostics;
using System.IO;

namespace DiscordData
{
    class DiscordDataBot
    {
        DiscordClient client;
        CommandService commands;
        List<User> users;
        String xmlfilepath = "C:\\Users\\Andrew Riggs\\GIT REPOS\\DiscordData\\XML\\userdata.xml";

        public DiscordDataBot()
        {
            if (File.Exists(xmlfilepath))
            {
                users = XmlHelper.ReadFromXmlFile<List<User>>(xmlfilepath);
            }
            else
            {
                users = new List<User>();
            }
            

            client = new DiscordClient(input =>
            {
                input.LogLevel = LogSeverity.Info;
                input.LogHandler = Log;
            });

            client.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = false;
            });

            commands = client.GetService<CommandService>();

            client.MessageReceived += async (s, e) =>
            {
                if (!e.Message.IsAuthor && !e.Message.Text.StartsWith("!"))
                {
                    if(isInUserList(e.User.Name) == false)
                    {
                        users.Add(new User(e.User.Name));
                        await e.Channel.SendMessage("Registered " + e.User.Name);
                    }

                    findUserInList(e.User.Name).totalMessCount++;
                    findUserInList(e.User.Name).totalWordCount += toWordCount(e.Message.Text);
                    findUserInList(e.User.Name).average();
                    XmlHelper.WriteToXmlFile<List<User>>(xmlfilepath, users);
                    await e.Channel.SendMessage(findUserInList(e.User.Name).avgWordCount.ToString());
                    await e.Channel.SendMessage(toWordCount(e.Message.Text).ToString());
                }
            };

            setupCommands();

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
            .Description("!show 'username' : Shows the stats of a specified user.")
            .Do(async (e) =>
            {
                await e.Channel.SendMessage("```User: " + e.GetArg("Username") + System.Environment.NewLine
                    + "Average Word Count Per Post: " + findUserInList(e.GetArg("Username")).avgWordCount + System.Environment.NewLine +
                    "Total Post Count: " + findUserInList(e.GetArg("Username")).totalMessCount + System.Environment.NewLine
                    + "Total Word Count: " + findUserInList(e.GetArg("Username")).totalWordCount + System.Environment.NewLine
                    + "```");
            });
        }

        public int toWordCount(String text)
        {
            int wordcount = 0, index = 0;

            while(index < text.Length)
            {
                while (index < text.Length && !char.IsWhiteSpace(text[index]))
                    index++;

                wordcount++;

                while (index < text.Length && char.IsWhiteSpace(text[index]))
                    index++;
            }

            return wordcount;

        }

        public Boolean isInUserList(String nm)
        {
            if(users == null)
            {
                return false;
            }
            
            for(int i = 0; i < users.Count; i++)
            {
                if(nm == users[i].name)
                {
                    return true;
                }
            }
            return false;
        }

        public User findUserInList(String nm)
        {
            if (users == null)
            {
                return null;
            }

            for (int i = 0; i < users.Count; i++)
            {
                if (nm == users[i].name)
                {
                    return users[i];
                }
            }
            return null;
        }

    }
}
