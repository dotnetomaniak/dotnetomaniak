using Kigg.DomainObjects;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class KnownSource: IKnownSource
    {
        public KnownSourceGrade Grade { get; set; }
        public string Url { get; set; }
    }
}