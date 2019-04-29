using Kigg.DomainObjects;

namespace Kigg.Infrastructure.EF.POCO
{
    public class KnownSource
    {
        public KnownSourceGrade Grade { get; set; }
        public string Url { get; set; }
    }
}