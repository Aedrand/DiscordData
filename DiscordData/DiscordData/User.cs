using System;

namespace DiscordData
{
    public class User
    {
        public String name { get; set; }
        public int avgWordCount { get; set; }
        public int totalMessCount { get; set; }
        public int totalWordCount { get; set; }

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
            this.avgWordCount = this.totalWordCount / this.totalMessCount;
        }

    }
}
