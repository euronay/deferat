namespace Deferat.Models
{
    public class Author
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Twitter { get; set; }
        public string Github { get; set; }
        public string Image { get; set; }
        public bool Featured { get; set; }
        public string Blurb { get; set; }
    }
}