using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using HtmlAgilityPack;
using JobOfferParser.Data;
using JobOfferParser.DB;

namespace JobOfferParser.Parsers
{
    public class PracujplCrawler : IParser, ICrawler
    {
        private readonly IOfferPersister _persister;

        public PracujplCrawler(IOfferPersister persister)
        {
            if (persister == null) throw new ArgumentNullException("persister");
            _persister = persister;
        }

        private static Offer ParseOffer(HtmlNode node)
        {
            if (node.SelectSingleNode("ul") != null)
                return Offer.Empty;

            string link = node.SelectSingleNode("div//a[1]").Attributes["href"].Value;
            var offer = new Offer
                            {
                                Link = string.Format("http://pracuj.pl{0}", link)
                            };

            var request = WebRequest.Create(offer.Link);
            using (var response = request.GetResponse().GetResponseStream())
            {
                var document = new HtmlDocument();                
                document.Load(response);

                var body = document.DocumentNode.SelectSingleNode("//body");
                offer.Text = body.InnerText;
            }
            var bytes = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(offer.Text));
            StringBuilder sb = new StringBuilder();
            Array.ForEach(bytes, b => sb.AppendFormat("{0:X2}", b));
            offer.Sha1 = sb.ToString();

            return offer;
        }

        public void Crawl()
        {
            var request = WebRequest.Create(@"http://www.pracuj.pl/praca/informatyka%20programowanie;c,3000015");
            using (var response = request.GetResponse().GetResponseStream())
            {
                if (response == null)
                    return;

                var document = new HtmlDocument();                
                document.Load(response);

                var nodes = document.DocumentNode.SelectNodes("//ul[@class='sRs']/li");
                foreach (var node in nodes)
                {                    
                    _persister.Persist(ParseOffer(node));
                }
            }
        }
    }
}