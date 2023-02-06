namespace BanLogger.Structs
{
    public class EmbedImage
    {
        public EmbedImage(string url)
        {
            this.url = url;
        }
        
        public string url { get; set; }
    }
}