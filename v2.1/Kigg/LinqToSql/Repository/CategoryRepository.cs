namespace Kigg.Repository.LinqToSql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DomainObjects;

    public class CategoryRepository : BaseRepository<ICategory, Category>, ICategoryRepository
    {
        public CategoryRepository(IDatabase database) : base(database)
        {
        }

        public CategoryRepository(IDatabaseFactory factory) : base(factory)
        {
        }

        public override void Add(ICategory entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Category category = (Category) entity;

            if (Database.CategoryDataSource.Any(c => c.Name == category.Name))
            {
                throw new ArgumentException("\"{0}\" category already exits. Specifiy a diffrent name.".FormatWith(category.Name), "entity");
            }

            category.UniqueName = UniqueNameGenerator.GenerateFrom(Database.CategoryDataSource, category.Name);

            base.Add(category);
        }

        public override void Remove(ICategory entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Category category = (Category) entity;

            Database.DeleteAll(Database.StoryViewDataSource.Where(v => v.Story.CategoryId == category.Id));
            Database.DeleteAll(Database.CommentSubscribtionDataSource.Where(cs => cs.Story.CategoryId == category.Id));
            Database.DeleteAll(Database.CommentDataSource.Where(c => c.Story.CategoryId == category.Id));
            Database.DeleteAll(Database.VoteDataSource.Where(v => v.Story.CategoryId == category.Id));
            Database.DeleteAll(Database.MarkAsSpamDataSource.Where(sp => sp.Story.CategoryId == category.Id));
            Database.DeleteAll(Database.StoryTagDataSource.Where(st => st.Story.CategoryId == category.Id));
            Database.DeleteAll(Database.StoryDataSource.Where(s => s.CategoryId == category.Id));

            base.Remove(category);
        }

        public virtual ICategory FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            return Database.CategoryDataSource.SingleOrDefault(c => c.Id == id);
        }

        public virtual ICategory FindByUniqueName(string uniqueName)
        {
            Check.Argument.IsNotEmpty(uniqueName, "uniqueName");

            return Database.CategoryDataSource.SingleOrDefault(c => c.UniqueName == uniqueName);
        }

        public virtual ICollection<ICategory> FindAll()
        {
            return Database.CategoryDataSource.OrderBy(c => c.CreatedAt).Cast<ICategory>().ToList().AsReadOnly();
        }
    }
}