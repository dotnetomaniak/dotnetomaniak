using System.Collections.Generic;
using JobOfferParser.Data;
using JobOfferParser.Parsers;

namespace JobOfferParser.Crawlers
{
    public class PracujPlCrawler : CrawlerBase
    {
        public PracujPlCrawler(IOfferPersister persister, IParser parser) : base(persister, parser, @"http://www.pracuj.pl/praca/informatyka%20programowanie;c,3000015", "//ul[@class='sRs']/li")
        {
        }

    }
}