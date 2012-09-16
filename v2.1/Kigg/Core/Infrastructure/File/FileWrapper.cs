namespace Kigg.Infrastructure
{
    using System.IO;
    using System.Text;

    public class FileWrapper : IFile
    {
        public string ReadAllText(string path)
        {
            return File.ReadAllText(path, Encoding.UTF8);
        }

        public string[] ReadAllLine(string path)
        {
            return File.ReadAllLines(path);
        }

        public void WriteAllText(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}
