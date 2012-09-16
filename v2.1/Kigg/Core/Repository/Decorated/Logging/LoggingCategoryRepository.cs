namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;
    using Infrastructure;

    public class LoggingCategoryRepository : DecoratedCategoryRepository
    {
        public LoggingCategoryRepository(ICategoryRepository innerRepository) : base(innerRepository)
        {
        }

        public override void Add(ICategory entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Log.Info("Adding category: {0}, {1}", entity.Id, entity.Name);
            base.Add(entity);
            Log.Info("Category added: {0}, {1}", entity.Id, entity.Name);
        }

        public override void Remove(ICategory entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Log.Warning("Removing category: {0}, {1}", entity.Id, entity.Name);
            base.Remove(entity);
            Log.Warning("Category removed: {0}, {1}", entity.Id, entity.Name);
        }

        public override ICategory FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            Log.Info("Retrieving category with id: {0}", id);

            var result = base.FindById(id);

            if (result == null)
            {
                Log.Warning("Did not find any category with id: {0}", id);
            }
            else
            {
                Log.Info("Category retrieved with id: {0}", id);
            }

            return result;
        }

        public override ICategory FindByUniqueName(string uniqueName)
        {
            Check.Argument.IsNotEmpty(uniqueName, "uniqueName");

            Log.Info("Retrieving category with unique name: {0}", uniqueName);

            var result = base.FindByUniqueName(uniqueName);

            if (result == null)
            {
                Log.Warning("Did not find any category with unique name: {0}", uniqueName);
            }
            else
            {
                Log.Info("Category retrieved with unique name: {0}", uniqueName);
            }

            return result;
        }

        public override ICollection<ICategory> FindAll()
        {
            Log.Info("Retrieving all Category");

            var result = base.FindAll();

            if (result.IsNullOrEmpty())
            {
                Log.Warning("Did not find any category");
            }
            else
            {
                Log.Info("All category retrieved");
            }

            return result;
        }
    }
}