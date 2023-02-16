using System.Collections.Generic;

namespace BanLogger_NWApi.Structs
{
    public class Message
    {
        public Message()
        {
            username = Plugin.Singleton.Config.Username;
            avatar_url = Plugin.Singleton.Config.AvatarUrl;
            content = Plugin.Singleton.Config.Content;
            tts = Plugin.Singleton.Config.Tts;
            embeds = new List<Embed>();
        }
            
        public string username { get; set; }
        public  string avatar_url { get; set; }
        public  string content { get; set; }
        public bool tts { get; set;  }
        public List<Embed> embeds { get; set; }
    }
}