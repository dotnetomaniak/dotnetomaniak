using System.Collections.Generic;
using HtmlAgilityPack;

namespace JobOfferParser.Data
{
    public interface IParser
    {
        Offer ParseOffer(HtmlNode node);
    }
}