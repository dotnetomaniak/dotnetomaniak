namespace Kigg.Repository
{
    using DomainObjects;

    public interface IKnownSourceRepository : IRepository<IKnownSource>
    {
        IKnownSource FindMatching(string url);
    }
}