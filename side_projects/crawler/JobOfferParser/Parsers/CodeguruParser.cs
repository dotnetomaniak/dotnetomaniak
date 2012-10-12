using System;
using System.Net;
using HtmlAgilityPack;
using JobOfferParser.Data;
using JobOfferParser.Helpers;

namespace JobOfferParser.Parsers
{
    public class CodeguruParser : IParser
    {
        public Offer ParseOffer(HtmlNode node)
        {
            string link = node.SelectSingleNode("a[1]").Attributes["href"].Value;
            string title = node.SelectSingleNode("div[@class='info']/div[1]/h3[1]/a[1]").InnerText;


            var offer = new Offer
            {
                Link = link,
                Title = title,
            };

            var date = node.SelectSingleNode("div[@class='description']/dl[1]/dd[1]/span[1]").InnerText;

            DateTime dateParsed;
            DateTime.TryParse(date, out dateParsed);
            offer.Date = dateParsed;
            var request = WebRequest.Create(offer.Link);

            using (var response = request.GetResponse().GetResponseStream())
            {
                var document = new HtmlDocument();
                document.Load(response);


                var body = document.DocumentNode.SelectSingleNode("//body");
                var textRawHtml = body.SelectSingleNode("//div[@class='divPostContent']/div[1]").InnerHtml;

                offer.City = "";
                offer.Province = "";
                offer.Text = textRawHtml;
                offer.Source = "CodeGuru";
            }

            offer.Sha1 = OfferHelper.GenerateSha1(offer.Text);
            return offer;
        }
    }
}