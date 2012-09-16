namespace Kigg.DomainObjects
{
    public interface IComment : IEntity
    {
        IStory ForStory
        {
            get;
        }

        string HtmlBody
        {
            get;
        }

        string TextBody
        {
            get;
        }

        IUser ByUser
        {
            get;
        }

        string FromIPAddress
        {
            get;
        }

        bool IsOffended
        {
            get;
        }

        void MarkAsOffended();
    }
}