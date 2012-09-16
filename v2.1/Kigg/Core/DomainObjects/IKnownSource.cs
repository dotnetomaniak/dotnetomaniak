namespace Kigg.DomainObjects
{
    public interface IKnownSource
    {
        string Url
        {
            get;
        }

        KnownSourceGrade Grade
        {
            get;
            set;
        }
    }
}