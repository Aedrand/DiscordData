using System;
using System.Collections.Generic;
using Discord;
using Discord.Commands;
using System.IO;
using MoreLinq;

namespace DiscordData
{
    class DiscordDataBot
    {
        DiscordClient client;
        CommandService commands;
        List<User> users;
        string xmluserfilepath = "userdata.xml";


        public DiscordDataBot()
        {
            if (File.Exists(xmluserfilepath))
            {
                users = XmlHelper.ReadFromXmlFile<List<User>>(xmluserfilepath);
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
                    await e.Channel.SendMessage("K");
                    XmlHelper.WriteToXmlFile<List<User>>(xmluserfilepath, users);
                }
            };

            setupCommands();

            client.ExecuteAndWait(async () =>
            {
                await client.Connect("Mjc0NjU1MzU1OTUzMjE3NTM2.C21QRA.Qdw59O2Z7OVi58hKa4q8CNGbfEM", TokenType.Bot);
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

            commands.CreateCommand("awards")
            .Description("!awards : Shows the highest score in each category.")
            .Do(async (e) =>
            {
                User avgHigh = findHighestAvg();
                User wordHigh = findMostWords();
                User messHigh = findMostMess();

                await e.Channel.SendMessage("```Highest Average Word Count Per Post: " + avgHigh.avgWordCount + " ("
                    + avgHigh.name + ")" + System.Environment.NewLine + "Highest Word Count: " + wordHigh.totalWordCount
                    + " (" + wordHigh.name + ")" + System.Environment.NewLine + "Highest Message Count: "
                    + messHigh.totalMessCount + " (" + messHigh.name + ")```");
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

        public User findHighestAvg()
        {
            User user = users.MaxBy(x => x.avgWordCount);
            return user;
        }

        public User findMostWords()
        {
            User user = users.MaxBy(x => x.totalWordCount);
            return user;
        }

        public User findMostMess()
        {
            User user = users.MaxBy(x => x.totalMessCount);
            return user;
        }

    }
}
