namespace Kigg.LinqToSql.Repository
{
    using System;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;
    
    public class KnownSourceRepository : BaseRepository<IKnownSource, KnownSource>, IKnownSourceRepository
    {
        public KnownSourceRepository(IDatabase database) : base(database)
        {
        }

        public KnownSourceRepository(IDatabaseFactory factory) : base(factory)
        {
        }

        public override void Add(IKnownSource entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            KnownSource knownSource = (KnownSource) entity;

            if (Database.KnownSourceDataSource.Any(ks => ks.Url == knownSource.Url))
            {
                throw new ArgumentException("\"{0}\" source already exits. Specifiy a diffrent url.".FormatWith(knownSource.Url), "entity");
            }

            base.Add(knownSource);
        }

        public virtual IKnownSource FindMatching(string url)
        {
            const string SQL = "select [Url], [Grade] from (select [Url], [Grade], len([Url]) as [Length] from [KnownSource] where charindex([Url], '{0}') > 0) as [KnownSourceWithGrade] order by [Length] desc";

            Check.Argument.IsNotInvalidWebUrl(url, "url");

            return Database.ExecuteQuery<KnownSource>(SQL, url).FirstOrDefault();
        }
    }
}