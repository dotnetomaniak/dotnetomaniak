namespace Kigg.Infrastructure
{
    using System.Diagnostics;
    using System.IO;

    public class FileWrapper : IFile
    {
        [DebuggerStepThrough]
        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        [DebuggerStepThrough]
        public string[] ReadAllLine(string path)
        {
            return File.ReadAllLines(path);
        }

        [DebuggerStepThrough]
        public void WriteAllText(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}