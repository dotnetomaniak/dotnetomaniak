using System;
using JobOfferParser.Data;

namespace JobOfferParser.Parsers
{
    public class GoldenLineCrawler : IParser, ICrawler
    {
        private readonly IOfferPersister _offerPersister;

        public GoldenLineCrawler(IOfferPersister offerPersister)
        {
            if (offerPersister == null) throw new ArgumentNullException("offerPersister");
            _offerPersister = offerPersister;
        }

        public void Crawl()
        {
            
        }
    }
}