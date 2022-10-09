using YamlDotNet.Serialization;

namespace Deferat.Models
{
    public class Settings : IMetadata
    {
        public string Title { get; set; }

        [YamlMember(Alias = "navbar-bg", ApplyNamingConventions = false)]
        public string Navbar { get; set; }

        public string Hero { get; set; }
        public string HeroCredit { get; set; }
        public string Privacy { get; set; }
        public string Id { get; set; }
        public string Html { get; set; }
        public string CopyRight { get; set; }
    }
}