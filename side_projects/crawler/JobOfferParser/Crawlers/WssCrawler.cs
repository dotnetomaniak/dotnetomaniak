using System;
using JobOfferParser.Data;

namespace JobOfferParser.Crawlers
{
    public class WssCrawler : CrawlerBase
    {
        public WssCrawler(IOfferRepository repository, IParser parser) : base(repository, parser, "http://www.wss.pl/tag/forum/praca,223", "//div[@class='forumThreadsListElement']")
        {
        }

    }
}