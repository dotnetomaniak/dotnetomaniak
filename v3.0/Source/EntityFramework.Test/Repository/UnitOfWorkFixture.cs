using System;

using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    using Kigg.EF.Repository;

    public class UnitOfWorkFixture : EfBaseFixture
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkFixture()
        {
            _unitOfWork = new UnitOfWork(database.Object);
        }

        [Fact]
        public void Constructor_With_Factory_Should_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new UnitOfWork(databaseFactory.Object));
        }

        [Fact]
        public void Commit_Should_Use_Database_SubmitChanges()
        {
            database.Setup(d => d.SubmitChanges()).Verifiable();

            _unitOfWork.Commit();
        }

        [Fact]
        public void Commit_Should_Throw_Exception_If_Previously_Disposed()
        {
            _unitOfWork.Dispose();
            Assert.Throws<ObjectDisposedException>(() => _unitOfWork.Commit());
        }
    }
}