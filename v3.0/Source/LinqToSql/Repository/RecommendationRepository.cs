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

        //public virtual IRecommendation isVisible()
        //{
        //    Check.Argument.IsNotNull(visible, "visible");

        //    return Database.RecommendationDataSource.SingleOrDefault(v => v == );
        //}

        public IRecommendation FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual IRecommendation FindByRecommendationTitle(string recommendationTitle)
        {
            Check.Argument.IsNotEmpty(recommendationTitle, "recommendationTitle");

            return Database.RecommendationDataSource.SingleOrDefault(r => r.RecommendationTitle == recommendationTitle.Trim());
        }

        public IQueryable<IRecommendation> GetAll()
        {
            var now = SystemTime.Now();

            return Database.RecommendationDataSource
                .Where(r => r.StartTime < now && r.EndTime >= now)
                .OrderBy(r => r.CreatedAt);
        }

        //public virtual IRecommendation FindByRecommendationLink(string recommendationLink)
        //{
        //    Check.Argument.IsNotEmpty(recommendationLink, "recommendationLink");

        //    return Database.RecommendationDataSource.SingleOrDefault(r => r. == recommendationLink.Trim());
        //}
    }
}
