using Deferat.Models;

namespace Deferat.Services
{
    public interface IFileReader<T> where T : class, IMetadata
    {
        T ReadFile(string path);
    }
}