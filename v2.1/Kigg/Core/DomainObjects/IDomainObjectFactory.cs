namespace Kigg.DomainObjects
{
    public interface IDomainObjectFactory
    {
        IUser CreateUser(string userName, string email, string password);

        IKnownSource CreateKnownSource(string url);

        ICategory CreateCategory(string name);

        ITag CreateTag(string name);

        IStory CreateStory(ICategory forCategory, IUser byUser, string fromIPAddress, string title, string description, string url);
    }
}