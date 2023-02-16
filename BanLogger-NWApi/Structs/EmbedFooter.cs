namespace BanLogger_NWApi.Structs
{
    public class EmbedFooter
    {
        public EmbedFooter(string text, string iconUrl)
        {
            Text = text;
            IconUrl = iconUrl;
        }
            
        public string Text { get; set; }
        public string IconUrl { get; set; }
    }
}