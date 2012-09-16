namespace Kigg.Core.DomainObjects
{
    using Kigg.DomainObjects;

    public interface IPromoteSiteItem : IUniqueNameEntity
    {
        string Url { get; set; }
        string Text { get; set; }
    }
}
