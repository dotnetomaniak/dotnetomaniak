using System;
using System.Collections.Generic;
using Kigg.DomainObjects;
using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Repository;

    public class LoggingCategoryRepositoryFixture : DecoratedRepositoryFixture
    {
        private readonly Mock<ICategoryRepository> _innerRepository;
        private readonly LoggingCategoryRepository _loggingRepository;

        public LoggingCategoryRepositoryFixture()
        {
            _innerRepository = new Mock<ICategoryRepository>();
            _loggingRepository = new LoggingCategoryRepository(_innerRepository.Object);
        }

        [Fact]
        public void Add_Should_Log_Info()
        {
            Add();

            log.Verify();
        }

        [Fact]
        public void Add_Should_Use_InnerRepository()
        {
            Add();

            _innerRepository.Verify();
        }

        [Fact]
        public void Remove_Should_Log_Warning()
        {
            Remove();

            log.Verify();
        }

        [Fact]
        public void Remove_Should_Use_InnerRepository()
        {
            Remove();

            _innerRepository.Verify();
        }

        [Fact]
        public void FindById_Should_Log_Info_When_Category_Exists()
        {
            FindById(CreateStubCategory());

            log.Verify();
        }

        [Fact]
        public void FindById_Should_Log_Warning_When_Category_Does_Not_Exist()
        {
            FindById(null);

            log.Verify();
        }

        [Fact]
        public void FindById_Should_Use_InnerRepository()
        {
            FindById(CreateStubCategory());

            _innerRepository.Verify();
        }

        [Fact]
        public void FindByUniqueName_Should_Log_Info_When_Category_Exists()
        {
            FindByUniqueName(CreateStubCategory());

            log.Verify();
        }

        [Fact]
        public void FindByUniqueName_Should_Log_Warning_When_Category_Does_Not_Exist()
        {
            FindByUniqueName(null);

            log.Verify();
        }

        [Fact]
        public void FindByUniqueName_Should_Use_InnerRepository()
        {
            FindByUniqueName(CreateStubCategory());

            _innerRepository.Verify();
        }

        [Fact]
        public void FindAll_Should_Log_Info_When_Categories_Exist()
        {
            FindAll(new List<ICategory> { CreateStubCategory() });

            log.Verify();
        }

        [Fact]
        public void FindByAll_Should_Log_Warning_When_Categories_Do_Not_Exist()
        {
            FindAll(null);

            log.Verify();
        }

        [Fact]
        public void FindAll_Should_Use_InnerRepository()
        {
            FindAll(new List<ICategory> { CreateStubCategory() });

            _innerRepository.Verify();
        }

        private void Add()
        {
            _innerRepository.Setup(r => r.Add(It.IsAny<ICategory>())).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.Add(CreateStubCategory());
        }

        private void Remove()
        {
            _innerRepository.Setup(r => r.Remove(It.IsAny<ICategory>())).Verifiable();
            log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();

            _loggingRepository.Remove(CreateStubCategory());
        }

        private void FindById(ICategory result)
        {
            _innerRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns(result).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindById(Guid.NewGuid());
        }

        private void FindByUniqueName(ICategory result)
        {
            _innerRepository.Setup(r => r.FindByUniqueName(It.IsAny<string>())).Returns(result).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindByUniqueName("acategory");
        }

        private void FindAll(ICollection<ICategory> result)
        {
            _innerRepository.Setup(r => r.FindAll()).Returns(result).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindAll();
        }
    }
}