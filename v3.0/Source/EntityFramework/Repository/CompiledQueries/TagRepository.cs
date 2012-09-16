namespace Kigg.EF.Repository
{
    using System;
    using System.Linq;
    using System.Data.Objects;

    using DomainObjects;

    public partial class TagRepository
    {
        private static readonly Func<Database, Guid, Tag>
            FindByIdQuery = CompiledQuery.Compile<Database, Guid, Tag>(
                (db, id) => db.TagSet.FirstOrDefault(t => t.Id == id));

        private static readonly Func<Database, string, Tag>
            FindByUniqueNameQuery = CompiledQuery.Compile<Database, string, Tag>(
                (db, uniqueName) => db.TagSet.FirstOrDefault(t => t.UniqueName == uniqueName));

        private static readonly Func<Database, string, Tag>
            FindByNameQuery = CompiledQuery.Compile<Database, string, Tag>(
                (db, name) => db.TagSet.FirstOrDefault(t => t.Name == name));
    
        private static readonly Func<Database, string, int, IQueryable<Tag>>
            FindMatchingQuery = CompiledQuery.Compile<Database, string, int, IQueryable<Tag>>(
                (db, name, max) => db.TagSet.Where(t => t.Name.StartsWith(name))
                                            .OrderBy(t => t.Name)
                                            .Take(max));

        private static readonly Func<Database, int, IQueryable<Tag>>
            FindByUsageQuery = CompiledQuery.Compile<Database, int, IQueryable<Tag>>(
                (db, top) => db.TagSet.Where(t => t.StoriesInternal.Any())
                                      .OrderByDescending(t => t.StoriesInternal.Count(st => st.ApprovedAt != null))
                                      .ThenBy(t => t.Name)
                                      .Take(top));

        private static readonly Func<Database, IQueryable<Tag>>
            FindAllQuery = CompiledQuery.Compile<Database, IQueryable<Tag>>(
                db => db.TagSet.OrderBy(t => t.Name));

    }
}
