using JobOfferParser.Data;

namespace JobOfferParser.Crawlers
{
    public class GumtreeCrawler : CrawlerBase
    {
        public GumtreeCrawler(IOfferPersister persister, IParser parser) : base(persister, parser, @"http://www.gumtree.pl/f-Praca-programisci-informatyka-internet-W0QQCatIdZ9005", "//td[@class=' hgk']")
        {
        }
    }
}