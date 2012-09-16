using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Infrastructure;
    using Repository;
    using Kigg.Test.Infrastructure;

    public class CreateDefaultUsersFixture : BaseFixture
    {
        private readonly Mock<IDomainObjectFactory> _factory;
        private readonly Mock<IUserRepository> _userRepository;

        private readonly IBootstrapperTask _task;

        public CreateDefaultUsersFixture()
        {
            _factory = new Mock<IDomainObjectFactory>();
            _userRepository = new Mock<IUserRepository>();

            DefaultUser[] _users = new [] 
                                            {
                                                new DefaultUser{UserName = "admin", Password = "admin", Email = "admin@dotnetshoutout.com", Role = Roles.Administrator},
                                                new DefaultUser{UserName = "support", Password = "support", Email = "support@dotnetshoutout.com", Role = Roles.Moderator}
                                            };

            _task = new CreateDefaultUsers(_factory.Object, _userRepository.Object, _users);
        }

        [Fact]
        public void Excecute_Should_Create_Users()
        {
            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns((IUser) null).Verifiable();
            _factory.Setup(f => f.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new Mock<IUser>().Object).Verifiable();
            _userRepository.Setup(r => r.Add(It.IsAny<IUser>())).Verifiable();
            unitOfWork.Setup(u => u.Commit()).Verifiable();

            _task.Execute();

            _factory.Verify();
            _userRepository.Verify();
            unitOfWork.Verify();
        }
    }
}