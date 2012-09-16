namespace Kigg.Infrastructure
{
    using DomainObjects;
    using Repository;

    public class CreateDefaultUsers : IBootstrapperTask
    {
        private readonly IDomainObjectFactory _factory;
        private readonly IUserRepository _userRepository;
        private readonly DefaultUser[] _users;

        public CreateDefaultUsers(IDomainObjectFactory factory, IUserRepository userRepository, DefaultUser[] users)
        {
            Check.Argument.IsNotNull(factory, "factory");
            Check.Argument.IsNotNull(userRepository, "userRepository");

            _factory = factory;
            _userRepository = userRepository;
            _users = users;
        }

        public void Execute()
        {
            if (!_users.IsNullOrEmpty())
            {
                using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                {
                    _users.ForEach(CreateUserIfNotExists);

                    unitOfWork.Commit();
                }
            }
        }

        private void CreateUserIfNotExists(DefaultUser defaultUser)
        {
            if (_userRepository.FindByUserName(defaultUser.UserName) == null)
            {
                IUser user = _factory.CreateUser(defaultUser.UserName, defaultUser.Email, defaultUser.Password);

                user.Role = defaultUser.Role;
                user.IsActive = true;

                _userRepository.Add(user);
            }
        }
    }
}