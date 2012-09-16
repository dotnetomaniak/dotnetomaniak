namespace Kigg.Repository
{
    using System;

    using DomainObjects;
    using Infrastructure;

    public class LoggingStoryRepository : DecoratedStoryRepository
    {
        public LoggingStoryRepository(IStoryRepository innerRepository) : base(innerRepository)
        {
        }

        public override void Add(IStory entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Log.Info("Adding story: {0}, {1}", entity.Id, entity.Title);
            base.Add(entity);
            Log.Info("Story added: {0}, {1}", entity.Id, entity.Title);
        }

        public override void Remove(IStory entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Log.Warning("Removing story: {0}, {1}", entity.Id, entity.Title);
            base.Remove(entity);
            Log.Warning("Story removed: {0}, {1}", entity.Id, entity.Title);
        }

        public override IStory FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            Log.Info("Retrieving story with id: {0}", id);

            var result = base.FindById(id);

            if (result == null)
            {
                Log.Warning("Did not find any story with id: {0}", id);
            }
            else
            {
                Log.Info("Story retrieved with id: {0}", id);
            }

            return result;
        }

        public override IStory FindByUniqueName(string uniqueName)
        {
            Check.Argument.IsNotEmpty(uniqueName, "uniqueName");

            Log.Info("Retrieving story with unique name: {0}", uniqueName);

            var result = base.FindByUniqueName(uniqueName);

            if (result == null)
            {
                Log.Warning("Did not find any story with unique name: {0}", uniqueName);
            }
            else
            {
                Log.Info("Story retrieved with unique name: {0}", uniqueName);
            }

            return result;
        }

        public override IStory FindByUrl(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            Log.Info("Retrieving story with url: {0}", url);

            var result = base.FindByUrl(url);

            if (result == null)
            {
                Log.Warning("Did not find any story with url: {0}", url);
            }
            else
            {
                Log.Info("Story retrieved with url: {0}", url);
            }

            return result;
        }

        public override PagedResult<IStory> FindPublished(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving published stories : {0}, {1}", start, max);

            var pagedResult = base.FindPublished(start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any published story : {0}, {1}", start, max);
            }
            else
            {
                Log.Info("Published stories retrieved : {0}, {1}", start, max);
            }

            return pagedResult;
        }

        public override PagedResult<IStory> FindPublishedByCategory(Guid categoryId, int start, int max)
        {
            Check.Argument.IsNotEmpty(categoryId, "categoryId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving published stories for category : {0}, {1}, {2}", categoryId, start, max);

            var pagedResult = base.FindPublishedByCategory(categoryId, start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any published story for category : {0}, {1}, {2}", categoryId, start, max);
            }
            else
            {
                Log.Info("Published stories retrieved for category : {0}, {1}, {2}", categoryId, start, max);
            }

            return pagedResult;
        }

        public override PagedResult<IStory> FindUpcoming(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving upcoming stories : {0}, {1}", start, max);

            var pagedResult = base.FindUpcoming(start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any upcoming story : {0}, {1}", start, max);
            }
            else
            {
                Log.Info("Upcoming stories retrieved : {0}, {1}", start, max);
            }

            return pagedResult;
        }

        public override PagedResult<IStory> FindNew(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving new stories : {0}, {1}", start, max);

            var pagedResult = base.FindNew(start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any new story : {0}, {1}", start, max);
            }
            else
            {
                Log.Info("New stories retrieved : {0}, {1}", start, max);
            }

            return pagedResult;
        }

        public override PagedResult<IStory> FindUnapproved(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving upapproved stories : {0}, {1}", start, max);

            var pagedResult = base.FindUnapproved(start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any upapproved story : {0}, {1}", start, max);
            }
            else
            {
                Log.Info("Upapproved stories retrieved : {0}, {1}", start, max);
            }

            return pagedResult;
        }

        public override PagedResult<IStory> FindPublishable(DateTime minimumDate, DateTime maximumDate, int start, int max)
        {
            Check.Argument.IsNotInFuture(minimumDate, "minimumDate");
            Check.Argument.IsNotInFuture(maximumDate, "maximumDate");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving publishable stories : {0}, {1}", start, max);

            var pagedResult = base.FindPublishable(minimumDate, maximumDate, start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any publishable story : {0}, {1}, {2}, {3}", minimumDate, maximumDate, start, max);
            }
            else
            {
                Log.Info("Publishable stories retrieved : {0}, {1}, {2}, {3}", minimumDate, maximumDate, start, max);
            }

            return pagedResult;
        }

        public override PagedResult<IStory> FindByTag(Guid tagId, int start, int max)
        {
            Check.Argument.IsNotEmpty(tagId, "tagId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving stories for tag : {0}, {1}, {2}", tagId, start, max);

            var pagedResult = base.FindByTag(tagId, start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any story for tag : {0}, {1}, {2}", tagId, start, max);
            }
            else
            {
                Log.Info("Stories retrieved for tag : {0}, {1}, {2}", tagId, start, max);
            }

            return pagedResult;
        }

        public override PagedResult<IStory> Search(string query, int start, int max)
        {
            Check.Argument.IsNotEmpty(query, "query");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Searching stories for query : {0}, {1}, {2}", query, start, max);

            var pagedResult = base.Search(query, start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any story for query : {0}, {1}, {2}", query, start, max);
            }
            else
            {
                Log.Info("Stories retrieved for query : {0}, {1}, {2}", query, start, max);
            }

            return pagedResult;
        }

        public override PagedResult<IStory> FindPostedByUser(Guid userId, int start, int max)
        {
            Check.Argument.IsNotEmpty(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving stories posted by user : {0}, {1}, {2}", userId, start, max);

            var pagedResult = base.FindPostedByUser(userId, start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any story posted by user : {0}, {1}, {2}", userId, start, max);
            }
            else
            {
                Log.Info("Posted Stories retrieved for user : {0}, {1}, {2}", userId, start, max);
            }

            return pagedResult;
        }

        public override PagedResult<IStory> FindPromotedByUser(Guid userId, int start, int max)
        {
            Check.Argument.IsNotEmpty(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving stories promoted by user : {0}, {1}, {2}", userId, start, max);

            var pagedResult = base.FindPromotedByUser(userId, start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any story promoted by user : {0}, {1}, {2}", userId, start, max);
            }
            else
            {
                Log.Info("Promoted Stories retrieved for user : {0}, {1}, {2}", userId, start, max);
            }

            return pagedResult;
        }

        public override PagedResult<IStory> FindCommentedByUser(Guid userId, int start, int max)
        {
            Check.Argument.IsNotEmpty(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving stories commented by user : {0}, {1}, {2}", userId, start, max);

            var pagedResult = base.FindCommentedByUser(userId, start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any story commented by user : {0}, {1}, {2}", userId, start, max);
            }
            else
            {
                Log.Info("Commented Stories retrieved for user : {0}, {1}, {2}", userId, start, max);
            }

            return pagedResult;
        }

        public override int CountByPublished()
        {
            Log.Info("Retrieving published count");

            var result = base.CountByPublished();

            Log.Info("Published count retrieved");

            return result;
        }

        public override int CountByUpcoming()
        {
            Log.Info("Retrieving upcoming count");

            var result = base.CountByUpcoming();

            Log.Info("Upcoming count retrieved");

            return result;
        }

        public override int CountByCategory(Guid categoryId)
        {
            Check.Argument.IsNotEmpty(categoryId, "categoryId");

            Log.Info("Retrieving count for category : {0}", categoryId);

            var result = base.CountByCategory(categoryId);

            Log.Info("Count retrieved for category : {0}", categoryId);

            return result;
        }

        public override int CountByTag(Guid tagId)
        {
            Check.Argument.IsNotEmpty(tagId, "tagId");

            Log.Info("Retrieving count for tag : {0}", tagId);

            var result = base.CountByTag(tagId);

            Log.Info("Count retrieved for tag : {0}", tagId);

            return result;
        }

        public override int CountByNew()
        {
            Log.Info("Retrieving new count");

            var result = base.CountByNew();

            Log.Info("New count retrieved");

            return result;
        }

        public override int CountByUnapproved()
        {
            Log.Info("Retrieving unapproved count");

            var result = base.CountByUnapproved();

            Log.Info("Unapproved count retrieved");

            return result;
        }

        public override int CountByPublishable(DateTime minimumDate, DateTime maximumDate)
        {
            Check.Argument.IsNotInFuture(minimumDate, "minimumDate");
            Check.Argument.IsNotInFuture(maximumDate, "maximumDate");

            Log.Info("Retrieving publishable count: {0}-{1}", minimumDate, maximumDate);

            var result = base.CountByPublishable(minimumDate, maximumDate);

            Log.Info("Publishable count retrieved: {0}-{1}", minimumDate, maximumDate);

            return result;
        }

        public override int CountPostedByUser(Guid userId)
        {
            Log.Info("Retrieving posted by user count: {0}".FormatWith(userId));

            var result = base.CountPostedByUser(userId);

            Log.Info("posted by user count: {0}".FormatWith(userId));

            return result;
        }
    }
}