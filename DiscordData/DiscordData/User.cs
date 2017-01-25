using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordData
{
    class User
    {
        String name { get; set; }
        int avgWordCount { get; set; }
        int totalMessCount { get; set; }

        public User(String nm)
        {
            name = nm;
            avgWordCount = 0;
            totalMessCount = 0;
        }
    }
}
