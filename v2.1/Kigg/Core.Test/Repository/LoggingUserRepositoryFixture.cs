using System;
using System.Collections.Generic;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Repository;

    public class LoggingUserRepositoryFixture : DecoratedRepositoryFixture
    {
        private readonly Mock<IUserRepository> _innerRepository;
        private readonly LoggingUserRepository _loggingRepository;

        public LoggingUserRepositoryFixture()
        {
            _innerRepository = new Mock<IUserRepository>();

            _loggingRepository = new LoggingUserRepository(_innerRepository.Object);
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
        public void FindById_Should_Log_Info_When_User_Exists()
        {
            FindById(CreateStubUser());

            log.Verify();
        }

        [Fact]
        public void FindById_Should_Log_Warning_When_User_Does_Not_Exist()
        {
            FindById(null);

            log.Verify();
        }

        [Fact]
        public void FindById_Should_Use_InnerRepository()
        {
            FindById(CreateStubUser());

            _innerRepository.Verify();
        }

        [Fact]
        public void FindByUserName_Should_Log_Info_When_User_Exists()
        {
            FindByUserName(CreateStubUser());

            log.Verify();
        }

        [Fact]
        public void FindByUserName_Should_Log_Warning_When_User_Does_Not_Exists()
        {
            FindByUserName(null);

            log.Verify();
        }

        [Fact]
        public void FindByUserName_Should_Use_InnerRepository()
        {
            FindByUserName(CreateStubUser());

            _innerRepository.Verify();
        }

        [Fact]
        public void FindByEmail_Should_Log_Info_When_User_Exists()
        {
            FindByEmail(CreateStubUser());

            log.Verify();
        }

        [Fact]
        public void FindByEmail_Should_Log_Warning_When_User_Does_Not_Exists()
        {
            FindByEmail(null);

            log.Verify();
        }

        [Fact]
        public void FindByEmail_Should_Use_InnerRepository()
        {
            FindByEmail(CreateStubUser());

            _innerRepository.Verify();
        }

        [Fact]
        public void FindScoreById_Should_Log_Info()
        {
            FindScoreById();

            log.Verify();
        }

        [Fact]
        public void FindScoreById_Should_Use_InnerRepository()
        {
            FindScoreById();

            _innerRepository.Verify();
        }

        [Fact]
        public void FindTop_Should_Log_Info_When_Users_Exist()
        {
            FindTop(new[] { CreateStubUser() });

            log.Verify();
        }

        [Fact]
        public void FindTop_Should_Log_Warning_When_Users_Do_Not_Exist()
        {
            FindTop(null);

            log.Verify();
        }

        [Fact]
        public void FindTop_Should_Use_InnerRepository()
        {
            FindTop(new[] { CreateStubUser() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindAll_Should_Log_Info_When_Users_Exist()
        {
            FindAll(new[] { CreateStubUser() });

            log.Verify();
        }

        [Fact]
        public void FindAll_Should_Log_Warning_When_Users_Do_Not_Exist()
        {
            FindAll(null);

            log.Verify();
        }

        [Fact]
        public void FindAll_Should_Use_InnerRepository()
        {
            FindAll(new[] { CreateStubUser() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindIPAddresses_Should_Log_Info_When_IpAddresses_Exist()
        {
            FindIPAddresses(new[] { "192.168.0.1" });

            log.Verify();
        }

        [Fact]
        public void FindIPAddresses_Should_Log_Warning_When_IpAddresses_Do_Not_Exist()
        {
            FindIPAddresses(null);

            log.Verify();
        }

        [Fact]
        public void FindIPAddresses_Should_Use_InnerRepository()
        {
            FindIPAddresses(new []{ "192.168.0.1" });

            _innerRepository.Verify();
        }

        private void Add()
        {
            _innerRepository.Expect(r => r.Add(It.IsAny<IUser>())).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.Add(CreateStubUser());
        }

        private void Remove()
        {
            _innerRepository.Expect(r => r.Remove(It.IsAny<IUser>())).Verifiable();
            log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();

            _loggingRepository.Remove(CreateStubUser());
        }

        private void FindById(IUser result)
        {
            _innerRepository.Expect(r => r.FindById(It.IsAny<Guid>())).Returns(result).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindById(Guid.NewGuid());
        }

        private void FindByUserName(IUser result)
        {
            _innerRepository.Expect(r => r.FindByUserName(It.IsAny<string>())).Returns(result).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindByUserName("dummy");
        }

        private void FindByEmail(IUser result)
        {
            _innerRepository.Expect(r => r.FindByEmail(It.IsAny<string>())).Returns(result).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindByEmail("xxx@xxx.com");
        }

        private void FindScoreById()
        {
            _innerRepository.Expect(r => r.FindScoreById(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(100).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.FindScoreById(Guid.NewGuid(), SystemTime.Now().AddHours(-4), SystemTime.Now());
        }

        private void FindTop(ICollection<IUser> result)
        {
            _innerRepository.Expect(r => r.FindTop(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IUser>() : new PagedResult<IUser>(result, result.Count)).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindTop(SystemTime.Now().AddHours(-4), SystemTime.Now(), 0, 10);
        }

        private void FindAll(ICollection<IUser> result)
        {
            _innerRepository.Expect(r => r.FindAll(It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IUser>() : new PagedResult<IUser>(result, result.Count)).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindAll(0, 10);
        }

        private void FindIPAddresses(ICollection<string> result)
        {
            _innerRepository.Expect(r => r.FindIPAddresses(It.IsAny<Guid>())).Returns(result).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindIPAddresses(Guid.NewGuid());
        }
    }
}