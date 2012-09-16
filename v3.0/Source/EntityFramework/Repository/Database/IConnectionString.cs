namespace Kigg.EF.Repository
{
    public interface IConnectionString
    {
        string Value
        {
            get;
        }
        string ProviderName
        { 
            get;
        }
    }
}
