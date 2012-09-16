namespace Kigg.Web
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    using Infrastructure;

    public class BlockedIPCollection : DisposableResource, IBlockedIPCollection
    {
        private readonly IFile _file;
        private readonly string _path;
        private readonly List<string> _ipAddresses = new List<string>();

        public BlockedIPCollection(string fileName, IFile file)
        {
            Check.Argument.IsNotEmpty(fileName, "fileName");
            Check.Argument.IsNotNull(file, "file");

            _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            _file = file;

            Read();
        }

        public int Count
        {
            [DebuggerStepThrough]
            get { return _ipAddresses.Count; }
        }

        public bool IsReadOnly
        {
            [DebuggerStepThrough]
            get { return false; }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _ipAddresses.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string item)
        {
            Check.Argument.IsNotEmpty(item, "item");

            item = item.Trim();

            if (!_ipAddresses.Contains(item))
            {
                _ipAddresses.Add(item);
            }
        }

        public void Clear()
        {
            _ipAddresses.Clear();
        }

        public bool Contains(string item)
        {
            Check.Argument.IsNotEmpty(item, "item");

            return _ipAddresses.Contains(item.Trim());
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            _ipAddresses.CopyTo(array, arrayIndex);
        }

        public bool Remove(string item)
        {
            Check.Argument.IsNotEmpty(item, "item");

            item = item.Trim();

            return _ipAddresses.Contains(item) && _ipAddresses.Remove(item);
        }

        public void AddRange(ICollection<string> ipAddresses)
        {
            Check.Argument.IsNotEmpty(ipAddresses, "ipAddresses");

            foreach (string ip in ipAddresses)
            {
                Add(ip);
            }
        }

        public void RemoveRange(ICollection<string> ipAddresses)
        {
            Check.Argument.IsNotEmpty(ipAddresses, "ipAddresses");

            foreach (string ip in ipAddresses)
            {
                Remove(ip);
            }
        }

        protected override void Dispose(bool disposing)
        {
            Write();
            base.Dispose(disposing);
        }

        private void Read()
        {
            _ipAddresses.Clear();
            _ipAddresses.AddRange(_file.ReadAllLine(_path));
            _ipAddresses.RemoveAll(ip => string.IsNullOrEmpty(ip));
        }

        private void Write()
        {
            string content = string.Join(Environment.NewLine, _ipAddresses.ToArray());

            _file.WriteAllText(_path, content);
        }
    }
}