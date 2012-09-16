namespace Kigg.LinqToSql.Repository
{
    using System;

    public interface IDatabaseFactory : IDisposable
    {
        IDatabase Get();
    }
}