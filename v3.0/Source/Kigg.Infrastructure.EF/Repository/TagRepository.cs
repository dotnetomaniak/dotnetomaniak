using System;
using System.Collections.Generic;
using System.Linq;
using Kigg.DomainObjects;
using Kigg.Infrastructure.EF.DomainObjects;
using Kigg.Repository;

namespace Kigg.Infrastructure.EF.Repository
{
    public class TagRepository: BaseRepository<Tag>, ITagRepository
    {
        private readonly DotnetomaniakContext _context;

        public TagRepository(DotnetomaniakContext context) : base(context)
        {
            _context = context;
        }

        public void Add(ITag entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Tag tag = (Tag)entity;

            if (_context.Tags.Any(t => t.Name == tag.Name))
            {
                throw new ArgumentException("\"{0}\" tag already exits. Specifiy a diffrent name.".FormatWith(tag.Name), "entity");
            }

            tag.UniqueName = UniqueNameGenerator.GenerateFrom(_context.Tags, tag.Name);

            base.Add(tag);
        }

        public void Remove(ITag entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Tag tag = (Tag)entity;

            _context.RemoveRange(_context.StoryTags.Where(st => st.TagId == tag.Id));
            _context.RemoveRange(_context.UserTags.Where(ut => ut.TagId == tag.Id));

            base.Remove(tag);
        }

        public virtual ITag FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            return _context.Tags.SingleOrDefault(t => t.Id == id);
        }

        public virtual ITag FindByUniqueName(string uniqueName)
        {
            Check.Argument.IsNotEmpty(uniqueName, "uniqueName");

            return _context.Tags.SingleOrDefault(t => t.UniqueName == uniqueName);
        }

        public virtual ITag FindByName(string name)
        {
            Check.Argument.IsNotEmpty(name, "name");

            return _context.Tags.SingleOrDefault(t => t.Name == name);
        }

        public virtual ICollection<ITag> FindMatching(string name, int max)
        {
            Check.Argument.IsNotEmpty(name, "name");
            Check.Argument.IsNotNegativeOrZero(max, "max");

            return _context.Tags
                           .Where(t => t.Name.StartsWith(name))
                           .OrderBy(t => t.Name)
                           .Take(max)
                           .Cast<ITag>()
                           .ToList()
                           .AsReadOnly();
        }

        public virtual ICollection<ITag> FindByUsage(int top)
        {
            Check.Argument.IsNotNegativeOrZero(top, "top");

            return _context.Tags
                           .Where(t => t.StoryTags.Count > 0)
                           .OrderByDescending(t => t.StoryTags.Count(st => st.Story.ApprovedAt != null))
                           .ThenBy(t => t.Name)
                           .Take(top)
                           .Cast<ITag>()
                           .ToList()
                           .AsReadOnly();
        }

        public virtual ICollection<ITag> FindAll()
        {
            return _context.Tags
                           .OrderBy(t => t.Name)
                           .Cast<ITag>()
                           .ToList()
                           .AsReadOnly();
        }
    }
}