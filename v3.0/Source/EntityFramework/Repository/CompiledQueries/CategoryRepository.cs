namespace Kigg.EF.Repository
{
    using System;
    using System.Linq;
    using System.Data.Objects;
    
    using DomainObjects;

    public partial class CategoryRepository
    {
        private static readonly Func<Database, Guid, Category>
            FindByIdQuery = CompiledQuery.Compile<Database, Guid, Category>(
                (db, id) => db.CategorySet.FirstOrDefault(c => c.Id == id));

        private static readonly Func<Database, string, Category>
            FindByUniqueNameQuery = CompiledQuery.Compile<Database, string, Category>(
                (db, uniqueName) => db.CategorySet.FirstOrDefault(c => c.UniqueName == uniqueName));

        private static readonly Func<Database, IQueryable<Category>>
            FindAllQuery = CompiledQuery.Compile<Database, IQueryable<Category>>(
                db => db.CategorySet.OrderBy(c => c.CreatedAt));

    }
}
