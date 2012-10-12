using System;
using System.Collections.Generic;
using System.Net;
using HtmlAgilityPack;
using JobOfferParser.Data;

namespace JobOfferParser.Crawlers
{
    public abstract class CrawlerBase : ICrawler
    {
        private readonly IOfferPersister _persister;
        private readonly IParser _parser;
        private readonly string _requestUriString;
        private readonly string _nodesXPath;


        protected CrawlerBase(IOfferPersister persister, IParser parser, string requestUriString, string nodesXPath)
        {
            _persister = persister;
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
                document.Load(response);

                var nodes = document.DocumentNode.SelectNodes(_nodesXPath);
                
                foreach (var node in nodes)
                {
                    try
                    {
                        var offer = _parser.ParseOffer(node);
                        _persister.Persist(offer);
                    }
                    catch (Exception)
                    {

                    }                   
                }
            }

        }
    }
}