namespace Kigg.Web
{
    using System.Collections.Generic;

    public interface IBlockedIPCollection : ICollection<string>
    {
        void AddRange(ICollection<string> ipAddresses);

        void RemoveRange(ICollection<string> ipAddresses);
    }
}