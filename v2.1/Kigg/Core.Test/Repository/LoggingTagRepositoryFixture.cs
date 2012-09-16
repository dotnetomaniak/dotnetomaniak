using System;
using System.Collections.Generic;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Repository;

    public class LoggingTagRepositoryFixture : DecoratedRepositoryFixture
    {
        private readonly Mock<ITagRepository> _innerRepository;
        private readonly LoggingTagRepository _loggingRepository;

        public LoggingTagRepositoryFixture()
        {
            _innerRepository = new Mock<ITagRepository>();
            _loggingRepository = new LoggingTagRepository(_innerRepository.Object);
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
        public void FindById_Should_Log_Info_When_Tag_Exists()
        {
            FindById(CreateStubTag());

            log.Verify();
        }

        [Fact]
        public void FindById_Should_Log_Warning_When_Tag_Does_Not_Exist()
        {
            FindById(null);

            log.Verify();
        }

        [Fact]
        public void FindById_Should_Use_InnerRepository()
        {
            FindById(CreateStubTag());

            _innerRepository.Verify();
        }

        [Fact]
        public void FindByUniqueName_Should_Log_Info_When_Tag_Exists()
        {
            FindByUniqueName(CreateStubTag());

            log.Verify();
        }

        [Fact]
        public void FindByUniqueName_Should_Log_Warning_When_Tag_Does_Not_Exist()
        {
            FindByUniqueName(null);

            log.Verify();
        }

        [Fact]
        public void FindByUniqueName_Should_Use_InnerRepository()
        {
            FindByUniqueName(CreateStubTag());

            _innerRepository.Verify();
        }

        [Fact]
        public void FindByName_Should_Log_Info_When_Tag_Exists()
        {
            FindByName(CreateStubTag());

            log.Verify();
        }

        [Fact]
        public void FindByName_Should_Log_Warning_When_Tag_Does_Not_Exist()
        {
            FindByName(null);

            log.Verify();
        }

        [Fact]
        public void FindByName_Should_Use_InnerRepository()
        {
            FindByName(CreateStubTag());

            _innerRepository.Verify();
        }

        [Fact]
        public void FindMatching_Should_Log_Info_When_Tags_Exist()
        {
            FindMatching(new [] { CreateStubTag() });

            log.Verify();
        }

        [Fact]
        public void FindMatching_Should_Log_Warning_When_Tags_Do_Not_Exist()
        {
            FindMatching(null);

            log.Verify();
        }

        [Fact]
        public void FindMatching_Should_Use_InnerRepository()
        {
            FindMatching(new [] { CreateStubTag() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindByUsage_Should_Log_Info_When_Tags_Exist()
        {
            FindByUsage(new [] { CreateStubTag() });

            log.Verify();
        }

        [Fact]
        public void FindByUsage_Should_Log_Warning_When_Tags_Do_Not_Exist()
        {
            FindByUsage(null);

            log.Verify();
        }

        [Fact]
        public void FindByUsage_Should_Use_InnerRepository()
        {
            FindByUsage(new [] { CreateStubTag() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindAll_Should_Log_Info_When_Tags_Exist()
        {
            FindAll(new [] { CreateStubTag() });

            log.Verify();
        }

        [Fact]
        public void FindByAll_Should_Log_Warning_When_Tags_Do_Not_Exist()
        {
            FindAll(null);

            log.Verify();
        }

        [Fact]
        public void FindAll_Should_Use_InnerRepository()
        {
            FindAll(new [] { CreateStubTag() });

            _innerRepository.Verify();
        }

        private void Add()
        {
            _innerRepository.Expect(r => r.Add(It.IsAny<ITag>())).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.Add(CreateStubTag());
        }

        private void Remove()
        {
            _innerRepository.Expect(r => r.Remove(It.IsAny<ITag>())).Verifiable();
            log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();

            _loggingRepository.Remove(CreateStubTag());
        }

        private void FindById(ITag result)
        {
            _innerRepository.Expect(r => r.FindById(It.IsAny<Guid>())).Returns(result).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindById(Guid.NewGuid());
        }

        private void FindByUniqueName(ITag result)
        {
            _innerRepository.Expect(r => r.FindByUniqueName(It.IsAny<string>())).Returns(result).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindByUniqueName("atag");
        }

        private void FindByName(ITag result)
        {
            _innerRepository.Expect(r => r.FindByName(It.IsAny<string>())).Returns(result).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindByName("atag");
        }

        private void FindMatching(ICollection<ITag> result)
        {
            _innerRepository.Expect(r => r.FindMatching(It.IsAny<string>(), It.IsAny<int>())).Returns(result).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindMatching("xxx", 100);
        }

        private void FindByUsage(ICollection<ITag> result)
        {
            _innerRepository.Expect(r => r.FindByUsage(It.IsAny<int>())).Returns(result).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindByUsage(50);
        }

        private void FindAll(ICollection<ITag> result)
        {
            _innerRepository.Expect(r => r.FindAll()).Returns(result).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindAll();
        }
    }
}