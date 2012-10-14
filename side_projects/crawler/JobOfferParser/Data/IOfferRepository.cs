using System.Collections.Generic;
using System.Linq;

namespace JobOfferParser.Data
{
    public interface IOfferRepository
    {
        bool InsertOffer(Offer offer);
        IEnumerable<Keyword> GetAllKeywords();
        void InsertKeywordsForOffer(string offerSha1, IEnumerable<Keyword> keywords);
    }
}