using System;
using JobOfferParser.Data;

namespace JobOfferParser.Crawlers
{
    public class WssCrawler : CrawlerBase
    {
        public WssCrawler(IOfferPersister persister, IParser parser) : base(persister, parser, "http://www.wss.pl/tag/forum/praca,223", "//div[@class='forumThreadsListElement']")
        {
        }

    }
}