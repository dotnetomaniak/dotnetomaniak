using System;
using System.Linq;
using Kigg.Core.DomainObjects;
using Kigg.LinqToSql.DomainObjects;
using Kigg.Repository;

namespace Kigg.LinqToSql.Repository
{
    public class RecommendationRepository : BaseRepository<IRecommendation, Recommendation>, IRecommendationRepository
    {
        public RecommendationRepository(IDatabase database)
            : base(database)
        {
        }

        public RecommendationRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }

        public override void Add(IRecommendation entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Recommendation recommendation = (Recommendation) entity;

            base.Add(recommendation);
        }

        public override void Remove(IRecommendation entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Recommendation recommendation = (Recommendation) entity;

            //Database.DeleteAll(Database.CommentSubscribtionDataSource.Where(cs => cs.UserId == user.Id || cs.Story.UserId == user.Id));
            base.Remove(recommendation);
        }

        public virtual IRecommendation FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            return Database.RecommendationDataSource.SingleOrDefault(r => r.Id == id);
        }

        public virtual IRecommendation FindByRecommendationTitle(string recommendationTitle)
        {
            Check.Argument.IsNotEmpty(recommendationTitle, "recommendationTitle");

            return Database.RecommendationDataSource.SingleOrDefault(r => r.RecommendationTitle == recommendationTitle.Trim());
        }

        //public virtual IRecommendation FindByRecommendationLink(string recommendationLink)
        //{
        //    Check.Argument.IsNotEmpty(recommendationLink, "recommendationLink");

        //    return Database.RecommendationDataSource.SingleOrDefault(r => r. == recommendationLink.Trim());
        //}
    }
}
