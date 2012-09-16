using System.Data.Objects.DataClasses;

namespace Kigg.EF.DomainObjects
{
    internal static class EntityHelper
    {
        internal static void EnsureEntityCollection<TInterface, TEntity>(ref EntityCollection<TInterface, TEntity> entityCollection, EntityCollection<TEntity> originalEntityCollection)
            where TInterface : class
            where TEntity : class, IEntityWithRelationships, TInterface
        {

            if (entityCollection != null) return;

            Check.Argument.IsNotNull(originalEntityCollection, "originalEntityCollection");
            entityCollection = new EntityCollection<TInterface, TEntity>(originalEntityCollection);
        }

        internal static void EnsureEntityCollectionLoaded<TInterface>(IEntityCollection<TInterface> entityCollection)
            where TInterface : class
        {
            Check.Argument.IsNotNull(entityCollection, "entityCollection");

            if (entityCollection == null || entityCollection.IsLoaded) return;

            entityCollection.Load();
        }

        internal static void EnsureEntityReference<TInterface, TEntity>(ref EntityReference<TInterface, TEntity> entityRef, EntityReference<TEntity> originalEntityRef)
            where TInterface : class
            where TEntity : class, IEntityWithRelationships, TInterface
        {

            if (entityRef != null) return;

            Check.Argument.IsNotNull(originalEntityRef, "originalEntityRef");
            entityRef = new EntityReference<TInterface, TEntity>(originalEntityRef);
        }

        internal static void EnsureEntityReferenceLoaded<TInterface>(IEntityReference<TInterface> entityRef)
            where TInterface : class
        {
            Check.Argument.IsNotNull(entityRef, "entityRef");

            if (entityRef == null || entityRef.IsLoaded) return;

            entityRef.Load();
        }
    }
}
