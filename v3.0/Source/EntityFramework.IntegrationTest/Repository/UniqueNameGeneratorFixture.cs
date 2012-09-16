using Kigg.EF.Repository;
using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.DomainObjects;

    public class UniqueNameGeneratorFixture : BaseIntegrationFixture
    {
        [Fact]
        public void GenerateFrom_Should_Not_Throw_Exception()
        {
            var category = (Category)_domainFactory.CreateCategory("IoC/DI");
            
            #pragma warning disable 168
            var uniqueName = UniqueNameGenerator.GenerateFrom(_database.CategoryDataSource, category.Name);
            #pragma warning restore 168
        }
    }
}
