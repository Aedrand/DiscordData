using System;

namespace DiscordData
{
    public class User
    {
        public string name { get; set; }
        public double avgWordCount { get; set; }
        public double totalMessCount { get; set; }
        public double totalWordCount { get; set; }

        public User()
        {
            name = null;
            avgWordCount = 0;
            totalMessCount = 0;
            totalWordCount = 0;
        }

        public User(String nm)
        {
            name = nm;
            avgWordCount = 0;
            totalMessCount = 0;
            totalWordCount = 0;
        }

        public void average()
        {
            avgWordCount = Math.Round(totalWordCount / totalMessCount, 2);
        }

    }
}
