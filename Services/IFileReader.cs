namespace Deferat.Services
{
    public interface IFileReader
    {
        (string MetaData, string Text) ReadFile(string path);
    }
}