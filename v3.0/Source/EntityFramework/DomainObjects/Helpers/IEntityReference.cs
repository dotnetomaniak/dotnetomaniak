namespace Kigg.EF.DomainObjects
{
    public interface IEntityReference<TInterface> where TInterface : class
    {
        TInterface Value { get; }
        bool IsLoaded { get; }
        void Load();
        void Attach(TInterface entity);
        
    }
}
