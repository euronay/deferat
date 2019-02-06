namespace Deferat.Services
{
    public interface IFormatterService
    {
        string CreateTruncatedContent(string html, int maxLength);
        string FixImages(string html, string folder);
    }
}