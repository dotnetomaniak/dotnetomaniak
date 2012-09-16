namespace Kigg.Infrastructure
{
    public interface IHtmlSanitizer
    {
        string Sanitize(string html);
    }
}