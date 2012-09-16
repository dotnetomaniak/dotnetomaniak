namespace Kigg.Infrastructure
{
    using System;

    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}