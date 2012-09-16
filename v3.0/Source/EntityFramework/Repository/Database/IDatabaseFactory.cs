namespace Kigg.EF.Repository
{
    using System;

    public interface IDatabaseFactory : IDisposable
    {
        IDatabase Create();
    }
}