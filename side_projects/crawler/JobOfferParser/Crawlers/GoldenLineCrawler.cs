using System;
using JobOfferParser.Data;

namespace JobOfferParser.Crawlers
{
    public class GoldenlineCrawler : CrawlerBase
    {
        public GoldenlineCrawler(IOfferRepository repository, IParser parser) : base(repository, parser, "http://www.goldenline.pl/praca/informatyka-programowanie", "//tbody/tr")
        {
        }

    }
}