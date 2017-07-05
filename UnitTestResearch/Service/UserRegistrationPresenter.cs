namespace Service
{
    using DataAccess;

    using Domain;

    public class UserRegistrationPresenter
    {
        private readonly IUserRepository userRepository;

        private readonly IUserView userView;

        public UserRegistrationPresenter(IUserView userView, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.userView = userView;
        }

        public void RegisterUser(User user)
        {
            if (user == null)
            {
                userView.Saved = false;
            }
            else
            {
                userRepository.Save(user);
                userView.Saved = true;
            }
        }
    }
}
