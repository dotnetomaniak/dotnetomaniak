namespace Kigg.EF.Repository
{
    using System;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;

    public class KnownSourceRepository : BaseRepository<IKnownSource, KnownSource>, IKnownSourceRepository
    {
        public KnownSourceRepository(IDatabase database)
            : base(database)
        {
        }

        public KnownSourceRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }

        public override void Add(IKnownSource entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            var knownSource = (KnownSource)entity;

            if (Database.KnownSourceDataSource.Any(ks => ks.Url == knownSource.Url))
            {
                throw new ArgumentException("\"{0}\" source already exits. Specifiy a diffrent url.".FormatWith(knownSource.Url), "entity");
            }

            base.Add(knownSource);
        }
#if(DEBUG)
        public virtual IKnownSource FindMatching(string url)
#else
        public IKnownSource FindMatching(string url)
#endif
        {
            Check.Argument.IsNotNull(url, "url");
            Check.Argument.IsNotEmpty(url, "url");
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            return Database.KnownSourceDataSource.Where(k => url.IndexOf(k.Url) > -1)
                                                 .OrderByDescending(k=>k.Url)
                                                 .FirstOrDefault();
        }
    }
}