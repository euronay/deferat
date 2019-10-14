using Deferat.Models;

namespace Deferat.Services
{
    public interface ISiteInfo
    {
        string BasePath { get; }
        Settings Settings {get;}
    }
}