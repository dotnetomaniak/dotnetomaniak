using System;
using JobOfferParser.Data;

namespace JobOfferParser.Crawlers
{
    public class GoldenlineCrawler : CrawlerBase
    {
        public GoldenlineCrawler(IOfferPersister persister, IParser parser)
            : base(persister, parser, "http://www.goldenline.pl/praca/informatyka-programowanie", "//tbody/tr")
        {
        }

    }
}