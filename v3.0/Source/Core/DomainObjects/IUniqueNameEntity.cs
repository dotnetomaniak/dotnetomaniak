namespace Kigg.DomainObjects
{
    public interface IUniqueNameEntity : IEntity
    {
        string UniqueName
        {
            get;
        }
    }
}