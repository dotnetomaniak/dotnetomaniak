namespace Kigg.Infrastructure
{
    public interface IDependencyResolverFactory
    {
        IDependencyResolver CreateInstance();
    }
}