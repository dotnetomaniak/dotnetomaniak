namespace Kigg.Infrastructure
{
    public interface IThumbnail
    {
        string For(string url, ThumbnailSize inSize);

        void Capture(string url);
    }
}