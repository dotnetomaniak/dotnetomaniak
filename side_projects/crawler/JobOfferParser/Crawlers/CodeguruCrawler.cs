using System;
using JobOfferParser.Data;

namespace JobOfferParser.Crawlers
{
    public class CodeguruCrawler : CrawlerBase
    {
        public CodeguruCrawler(IOfferPersister persister, IParser parser) : base(persister, parser, "", "")
        {
        }
    }
}