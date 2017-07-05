namespace Service
{
    using DataAccess;

    using Domain;

    public class UserRegistrationPresenterWithRequiresSaving
    {
        private readonly IUserRepository userRepository;

        private readonly IUserView userView;

        public UserRegistrationPresenterWithRequiresSaving(IUserView studentView, IUserRepository studentRepository)
        {
            userRepository = studentRepository;
            userView = studentView;
        }

        public void RegisterUser(User user)
        {
            if (userView.Changed == true)
            {
                userRepository.Save(user);
                userView.Saved = true;
            }
            else
            {
                userView.Saved = false;
            }
        }
    }
}
