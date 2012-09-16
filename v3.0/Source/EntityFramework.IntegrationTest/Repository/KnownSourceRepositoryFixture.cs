using System;
using System.Data;
using System.Linq;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.DomainObjects;
    using Kigg.EF.Repository;

    public class KnownSourceRepositoryFixture : BaseIntegrationFixture
    {
        private readonly KnownSourceRepository _knownSourceRepository;

        public KnownSourceRepositoryFixture()
        {
            _knownSourceRepository = new KnownSourceRepository(_database);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new KnownSourceRepository(_dbFactory));
        }

        [Fact]
        public void Add_And_Presist_Changes_Should_Succeed()
        {
            using (BeginTransaction())
            {
                var knownSrc = (KnownSource)_domainFactory.CreateKnownSource("http://codebetter.com");
                _knownSourceRepository.Add(knownSrc);
                Assert.Equal(EntityState.Added, knownSrc.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Unchanged, knownSrc.EntityState);
            }
        }

        [Fact]
        public void Remove_And_Presist_Changes_Should_Succeed()
        {
            using (BeginTransaction())
            {
                var knownSrc = (KnownSource)_domainFactory.CreateKnownSource("http://codebetter.com");
                _database.InsertOnSubmit(knownSrc);       
                _database.SubmitChanges();
                
                _knownSourceRepository.Remove(knownSrc);
                Assert.Equal(EntityState.Deleted, knownSrc.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Detached, knownSrc.EntityState);
            }
        }

        [Fact]
        public void Add_Should_Throw_Exception_When_Specified_Url_Already_Exists()
        {
            using (BeginTransaction())
            {
                var knownSrc = (KnownSource)_domainFactory.CreateKnownSource("http://codebetter.com");
                _database.InsertOnSubmit(knownSrc);
                _database.SubmitChanges();

                Assert.Throws<ArgumentException>(() => _knownSourceRepository.Add(_domainFactory.CreateKnownSource(knownSrc.Url)));
            }

            
        }

        [Fact]
        public void FindMatching_Should_Return_Correct_KnownSource()
        {
            using (BeginTransaction())
            {
                var knownSrc = (KnownSource)_domainFactory.CreateKnownSource("http://codebetter.com");
                _knownSourceRepository.Add(knownSrc);
                _database.SubmitChanges();

                knownSrc = _database.KnownSourceDataSource.First();
                var url = knownSrc.Url + "/someuniquename";

                var knownSource = _knownSourceRepository.FindMatching(url);

                Assert.Equal(knownSrc.Url, knownSource.Url);
            }
            
        }
    }
}
