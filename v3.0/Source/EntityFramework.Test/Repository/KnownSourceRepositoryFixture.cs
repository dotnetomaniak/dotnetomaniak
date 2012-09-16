using System;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    using DomainObjects;
    using Kigg.EF.Repository;
    using Kigg.EF.DomainObjects;

    public class KnownSourceRepositoryFixture : EfBaseFixture
    {
        private readonly KnownSourceRepository _knownSourceRepository;
        private readonly IDomainObjectFactory _factory;

        public KnownSourceRepositoryFixture()
        {
            _factory = new DomainObjectFactory();
            _knownSourceRepository = new KnownSourceRepository(database.Object);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new KnownSourceRepository(databaseFactory.Object));
        }

        [Fact]
        public void Add_Should_Use_Database()
        {
            database.Setup(d => d.InsertOnSubmit(It.IsAny<KnownSource>())).Verifiable();

            _knownSourceRepository.Add(_factory.CreateKnownSource("http://weblog.asp.net"));
        }

        [Fact]
        public void Add_Should_Throw_Exception_When_Specified_Url_Already_Exists()
        {
            KnownSources.Add(_factory.CreateKnownSource("http://weblogs.asp.net") as KnownSource);

            Assert.Throws<ArgumentException>(() => _knownSourceRepository.Add(_factory.CreateKnownSource("http://weblogs.asp.net")));
        }

        [Fact]
        public void FindMatching_Should_Return_Correct_KnownSource()
        {
            KnownSources.Add(_factory.CreateKnownSource("http://weblogs.asp.net") as KnownSource);

            //database.Setup(db => db.ExecuteQuery<KnownSource>(It.IsAny<string>(), It.IsAny<object[]>())).Returns(new List<KnownSource> { KnownSources[0] }).Verifiable();

            var knownSource = _knownSourceRepository.FindMatching("http://weblogs.asp.net/ScottGu");

            Assert.Equal("http://weblogs.asp.net", knownSource.Url);
        }
    }
}