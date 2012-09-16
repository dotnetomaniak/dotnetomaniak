namespace Kigg.Repository.LinqToSql
{
    using System;

    public interface IDatabaseFactory : IDisposable
    {
        IDatabase Get();
    }
}