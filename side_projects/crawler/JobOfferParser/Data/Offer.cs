using System;
using System.Collections.ObjectModel;

namespace JobOfferParser.Data
{
    public struct Offer
    {
        public Collection<Keyword> Keywords { get; set; }
        public string Source { get; set; }
        public string Link { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Sha1 { get; set; }
        public DateTime Date { get; set; }

        public static Offer Empty
        {
            get { return new Offer();}
        }
    }
}