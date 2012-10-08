namespace JobOfferParser.Data
{
    public interface IOfferPersister
    {
        void Persist(Offer offer);
    }
}