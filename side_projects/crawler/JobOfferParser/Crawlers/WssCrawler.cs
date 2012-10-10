using System;
using JobOfferParser.Data;

namespace JobOfferParser.Crawlers
{
    public class WssCrawler : CrawlerBase
    {
        public WssCrawler(IOfferPersister persister, IParser parser) : base(persister, parser, "", "")
        {
        }

    }
}