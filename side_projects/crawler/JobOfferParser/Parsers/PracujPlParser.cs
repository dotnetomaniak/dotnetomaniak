using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using JobOfferParser.Data;
using JobOfferParser.Helpers;

namespace JobOfferParser.Parsers
{
    public class PracujPlParser : IParser 
    {
        public Offer ParseOffer(HtmlNode node)
        {
            if (node.SelectSingleNode("ul") != null)
                return Offer.Empty;


            string link = node.SelectSingleNode("div//a[@class='offerLink']").Attributes["href"].Value;           

            var offer = new Offer
            {
                Link = string.Format("http://pracuj.pl{0}", link),
            };

            var request = WebRequest.Create(offer.Link);

            using (var response = request.GetResponse().GetResponseStream())
            {
                var document = new HtmlDocument();
                document.Load(response, true);

                var body = document.DocumentNode.SelectSingleNode("//body");
                var rawHtmlText = body.SelectSingleNode("//div[@id='offCont']").InnerHtml;
               
                var dateRegex = new Regex("\\d{4}-\\d{2}-\\d{2}");

                var date = body.SelectNodes("//dd").First(x => dateRegex.IsMatch(x.InnerText)).InnerText;

                var addressContainer = body.SelectNodes("//dt").First(x => x.InnerText == "Lokalizacja:").NextSibling.NextSibling;


                string city = "";
                string province = "";

                string title = body.SelectSingleNode("//h1[@class='offerTitle']").InnerText;

                if(addressContainer.ChildNodes.Count > 3)
                {
                    city = addressContainer.ChildNodes.ElementAt(1).InnerText;
                    province = addressContainer.ChildNodes.ElementAt(3).InnerText;
                }
                else
                {
                    province = addressContainer.ChildNodes.ElementAt(0).InnerText;
                }


                DateTime dateParsed;
                DateTime.TryParse(date, out dateParsed);

                
                offer.Title = title;
                offer.Text = rawHtmlText;
                offer.City = city;
                offer.Province = province;                
                offer.Date = dateParsed;
                offer.Source = "PracujPL";

            }

            offer.Sha1 = OfferHelper.GenerateSha1(offer.Text);
            return offer;
        }
    }
}