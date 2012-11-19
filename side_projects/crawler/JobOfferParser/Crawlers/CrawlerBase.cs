using System;
using System.Collections.Generic;
using System.Net;
using HtmlAgilityPack;
using JobOfferParser.Data;
using JobOfferParser.Helpers;

namespace JobOfferParser.Crawlers
{
    public abstract class CrawlerBase : ICrawler
    {
        private readonly IOfferRepository _repository;
        private readonly IParser _parser;
        private readonly string _requestUriString;
        private readonly string _nodesXPath;


        protected CrawlerBase(IOfferRepository repository, IParser parser, string requestUriString, string nodesXPath)
        {
            _repository = repository;
            _parser = parser;
            _requestUriString = requestUriString;
            _nodesXPath = nodesXPath;
        }

        public virtual void Crawl()
        {
            var request = WebRequest.Create(_requestUriString);

            using (var response = request.GetResponse().GetResponseStream())
            {
                if (response == null)
                    return;

                var document = new HtmlDocument();
                document.Load(response, true);

                var nodes = document.DocumentNode.SelectNodes(_nodesXPath);
                var allKeywords = _repository.GetAllKeywords();
              

                foreach (var node in nodes)
                {
                    try
                    {
                        var offer = _parser.ParseOffer(node);
                        if(_repository.InsertOffer(offer))
                        {
                            var offerKeywords = OfferHelper.ScanTextForKeywords(offer.Text, allKeywords);
                            _repository.InsertKeywordsForOffer(offer.Sha1, offerKeywords);
                        }
                    }
                    catch (Exception ex)
                    {
                      var log = NLog.LogManager.GetCurrentClassLogger();
                      log.LogException(NLog.LogLevel.Fatal, "Exception occured", ex);
                    }                   
                }
            }

        }
    }
}