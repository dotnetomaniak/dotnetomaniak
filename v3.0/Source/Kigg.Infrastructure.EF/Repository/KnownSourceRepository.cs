using System;
using System.Linq;
using Kigg.DomainObjects;
using Kigg.Infrastructure.EF.DomainObjects;
using Kigg.Repository;
using Microsoft.EntityFrameworkCore;

namespace Kigg.Infrastructure.EF.Repository
{
    public class KnownSourceRepository: BaseRepository<KnownSource>, IKnownSourceRepository
    {
        private readonly DotnetomaniakContext _context;

        public KnownSourceRepository(DotnetomaniakContext context) : base(context)
        {
            _context = context;
        }

        public void Add(IKnownSource entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            KnownSource knownSource = (KnownSource)entity;

            if (_context.KnownSources.Any(ks => ks.Url == knownSource.Url))
            {
                throw new ArgumentException("\"{0}\" source already exits. Specifiy a diffrent url.".FormatWith(knownSource.Url), "entity");
            }

            base.Add(knownSource);
        }

        public void Remove(IKnownSource entity)
        {
            base.Remove((KnownSource)entity);
        }

        public IKnownSource FindMatching(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            return _context.KnownSources.Where(x => url.StartsWith(x.Url)).OrderByDescending(x => x.Url.Length).FirstOrDefault();
        }
    }
}