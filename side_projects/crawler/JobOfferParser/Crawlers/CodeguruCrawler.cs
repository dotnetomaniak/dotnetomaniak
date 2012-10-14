using System;
using JobOfferParser.Data;

namespace JobOfferParser.Crawlers
{
    public class CodeguruCrawler : CrawlerBase
    {
        public CodeguruCrawler(IOfferRepository repository, IParser parser) : base(repository, parser, "http://www.codeguru.pl/forum-board/praca---ogloszenia-na-codeguru,242", "//div[@class='forumThreadsListElement'][position()>1]")
        {
        }
    }
}