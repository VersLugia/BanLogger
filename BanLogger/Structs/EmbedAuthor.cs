namespace BanLogger.Structs
{
    public class EmbedAuthor
    {
        public EmbedAuthor(string name, string url, string iconUrl)
        {
            Name = name;
            Url = url;
            IconUrl = iconUrl;
        }
            
        public string Name { get; set; }
        public string Url { get; set; }
        public string IconUrl { get; set; }
    }
}