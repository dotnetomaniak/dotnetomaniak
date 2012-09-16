namespace Kigg.EF.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;

    public partial class CategoryRepository : BaseRepository<ICategory, Category>, ICategoryRepository
    {
        public CategoryRepository(IDatabase database)
            : base(database)
        {
        }

        public CategoryRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }

        public override void Add(ICategory entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            var category = (Category)entity;

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

            var category = (Category)entity;

            Database.DeleteAllOnSubmit(Database.StoryViewDataSource.Where(v => v.Story.Category.Id == category.Id));
            Database.DeleteAllOnSubmit(Database.CommentDataSource.Where(c => c.Story.Category.Id == category.Id));
            Database.DeleteAllOnSubmit(Database.VoteDataSource.Where(v => v.Story.Category.Id == category.Id));
            Database.DeleteAllOnSubmit(Database.MarkAsSpamDataSource.Where(sp => sp.Story.Category.Id == category.Id));

            var stories = Database.StoryDataSource.Where(s => s.Category.Id == category.Id).AsEnumerable();

            foreach (var story in stories)
            {
                story.CommentSubscribers.Clear();
                story.StoryTags.Clear();
            }
            Database.DeleteAllOnSubmit(stories);

            base.Remove(category);
        }

#if(DEBUG)
        public virtual ICategory FindById(Guid id)
#else
        public ICategory FindById(Guid id)
#endif
        {
            Check.Argument.IsNotEmpty(id, "id");

            return (DataContext != null)
                    ? FindByIdQuery.Invoke(DataContext, id) 
                    : Database.CategoryDataSource.FirstOrDefault(c => c.Id == id);
        }

#if(DEBUG)
        public virtual ICategory FindByUniqueName(string uniqueName)
#else
        public ICategory FindByUniqueName(string uniqueName)
#endif

        {
            Check.Argument.IsNotEmpty(uniqueName, "uniqueName");
            return (DataContext != null)
                    ? FindByUniqueNameQuery.Invoke(DataContext, uniqueName) 
                    : Database.CategoryDataSource.FirstOrDefault(c => c.UniqueName == uniqueName);
        }

#if(DEBUG)
        public virtual ICollection<ICategory> FindAll()
#else
        public ICollection<ICategory> FindAll()
#endif
        
        {
            var categories = (DataContext != null)
                                 ? FindAllQuery.Invoke(DataContext)
                                 : Database.CategoryDataSource.OrderBy(c => c.CreatedAt);
            
            return categories.AsEnumerable()
                             .Cast<ICategory>()
                             .ToList()
                             .AsReadOnly();
        }
    }
}