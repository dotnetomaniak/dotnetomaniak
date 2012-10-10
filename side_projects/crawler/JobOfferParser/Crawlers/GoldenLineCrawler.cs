using System;
using JobOfferParser.Data;

namespace JobOfferParser.Crawlers
{
    public class GoldenLineCrawler : CrawlerBase
    {
        public GoldenLineCrawler(IOfferPersister persister, IParser parser) : base(persister, parser, "", "")
        {
        }

    }
}