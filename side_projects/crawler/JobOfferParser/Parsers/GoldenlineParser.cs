using System;
using System.Net;
using HtmlAgilityPack;
using JobOfferParser.Data;
using JobOfferParser.Helpers;

namespace JobOfferParser.Parsers
{
    public class GoldenlineParser : IParser
    {
        public Offer ParseOffer(HtmlNode node)
        {
            string link = node.SelectSingleNode("td[@class='position']").SelectSingleNode("a[1]").Attributes["href"].Value;
            var dateAndProvince = node.SelectSingleNode("td[@class='date']").InnerHtml.Split(new string[] { "<br>" }, StringSplitOptions.None);

            var date = dateAndProvince[0];
            var province = dateAndProvince[1];

            DateTime dateParsed;
            DateTime.TryParse(date, out dateParsed);


            var offer = new Offer
            {
                Link = link,
                Date = dateParsed,
                Province = province,
            };

            var request = WebRequest.Create(offer.Link);

            using (var response = request.GetResponse().GetResponseStream())
            {
                var document = new HtmlDocument();
                document.Load(response);

                

                var body = document.DocumentNode.SelectSingleNode("//body");
                var title = document.DocumentNode.SelectSingleNode("//strong[1]").InnerText;
                var textRawHtml = body.InnerHtml;

                offer.City = "";
                offer.Title = title;
                offer.Text = textRawHtml;
                offer.Source = "GoldenLine";
            }

            offer.Sha1 = OfferHelper.GenerateSha1(offer.Text);
            return offer;
        }
    }
}