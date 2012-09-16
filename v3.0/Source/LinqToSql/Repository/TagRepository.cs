namespace Kigg.LinqToSql.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;
    
    public class TagRepository : BaseRepository<ITag, Tag>, ITagRepository
    {
        public TagRepository(IDatabase database) : base(database)
        {
        }

        public TagRepository(IDatabaseFactory factory) : base(factory)
        {
        }

        public override void Add(ITag entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Tag tag = (Tag) entity;

            if (Database.TagDataSource.Any(t => t.Name == tag.Name))
            {
                throw new ArgumentException("\"{0}\" tag already exits. Specifiy a diffrent name.".FormatWith(tag.Name), "entity");
            }

            tag.UniqueName = UniqueNameGenerator.GenerateFrom(Database.TagDataSource, tag.Name);

            Database.Insert(tag);
        }

        public override void Remove(ITag entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Tag tag = (Tag) entity;

            Database.DeleteAll(Database.StoryTagDataSource.Where(st => st.TagId == tag.Id));
            Database.DeleteAll(Database.UserTagDataSource.Where(ut => ut.TagId == tag.Id));

            base.Remove(tag);
        }

        public virtual ITag FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            return Database.TagDataSource.SingleOrDefault(t => t.Id == id);
        }

        public virtual ITag FindByUniqueName(string uniqueName)
        {
            Check.Argument.IsNotEmpty(uniqueName, "uniqueName");

            return Database.TagDataSource.SingleOrDefault(t => t.UniqueName == uniqueName);
        }

        public virtual ITag FindByName(string name)
        {
            Check.Argument.IsNotEmpty(name, "name");

            return Database.TagDataSource.SingleOrDefault(t => t.Name == name);
        }

        public virtual ICollection<ITag> FindMatching(string name, int max)
        {
            Check.Argument.IsNotEmpty(name, "name");
            Check.Argument.IsNotNegativeOrZero(max, "max");

            return Database.TagDataSource
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

            return Database.TagDataSource
                           .Where(t => t.StoryTags.Count  > 0)
                           .OrderByDescending(t => t.StoryTags.Count(st => st.Story.ApprovedAt != null))
                           .ThenBy(t => t.Name)
                           .Take(top)
                           .Cast<ITag>()
                           .ToList()
                           .AsReadOnly();
        }

        public virtual ICollection<ITag> FindAll()
        {
            return Database.TagDataSource
                           .OrderBy(t => t.Name)
                           .Cast<ITag>()
                           .ToList()
                           .AsReadOnly();
        }
    }
}