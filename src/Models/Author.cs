namespace Deferat.Models
{
    public class Author : IMetadata
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Twitter { get; set; }
        public string Github { get; set; }
        public string LinkedIn { get; set; }
        public string Image { get; set; }
        public string ImageUrl
        {
            get => $"authors/{Id}/{Image}";
        }
        public bool Featured { get; set; }
        public string Html { get; set; }
    }
}