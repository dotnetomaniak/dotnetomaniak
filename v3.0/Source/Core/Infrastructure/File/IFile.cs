namespace Kigg.Infrastructure
{
    public interface IFile
    {
        string ReadAllText(string path);

        string[] ReadAllLine(string path);

        void WriteAllText(string path, string content);
    }
}