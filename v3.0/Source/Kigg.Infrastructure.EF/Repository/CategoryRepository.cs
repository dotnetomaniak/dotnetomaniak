using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Kigg.DomainObjects;
using Kigg.Infrastructure.EF.DomainObjects;
using Kigg.Repository;

namespace Kigg.Infrastructure.EF.Repository
{
    public class CategoryRepository: BaseRepository<Category>, ICategoryRepository
    {
        private readonly DotnetomaniakContext _context;

        public CategoryRepository(DotnetomaniakContext context) : base(context)
        {
            _context = context;
        }

        public void Add(ICategory entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            var category = (Category)entity;

            if (_context.Categories.Any(c => c.Name == category.Name))
            {
                throw new ArgumentException("\"{0}\" category already exits. Specifiy a diffrent name.".FormatWith(category.Name), "entity");
            }

            category.UniqueName = UniqueNameGenerator.GenerateFrom(_context.Categories, category.Name);

            base.Add(category);
        }

        public void Remove(ICategory entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            var category = (Category)entity;

            _context.RemoveRange(_context.StoryViews.Where(v => v.Story.CategoryId == category.Id));
            _context.RemoveRange(_context.CommentSubscribtions.Where(cs => cs.Story.CategoryId == category.Id));
            _context.RemoveRange(_context.StoryComments.Where(c => c.Story.CategoryId == category.Id));
            _context.RemoveRange(_context.StoryVotes.Where(v => v.Story.CategoryId == category.Id));
            _context.RemoveRange(_context.StoryMarkAsSpams.Where(sp => sp.Story.CategoryId == category.Id));
            _context.RemoveRange(_context.StoryTags.Where(st => st.Story.CategoryId == category.Id));
            _context.RemoveRange(_context.Stories.Where(s => s.CategoryId == category.Id));

            base.Remove(category);
        }

        public ICategory FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            return _context.Categories.SingleOrDefault(c => c.Id == id);
        }

        public ICategory FindByUniqueName(string uniqueName)
        {
            Check.Argument.IsNotEmpty(uniqueName, "uniqueName");

            return _context.Categories.SingleOrDefault(c => c.UniqueName == uniqueName);

        }

        public ICollection<ICategory> FindAll()
        {
            return _context.Categories.OrderBy(c => c.CreatedAt).Cast<ICategory>().ToList().AsReadOnly();
        }
    }
}