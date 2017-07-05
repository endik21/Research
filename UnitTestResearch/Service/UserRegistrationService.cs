namespace Service
{
    using System;
    using System.Net;

    using DataAccess;

    using Domain;

    public class UserRegistrationService
    {
        private IUserRepository repository = null;

        public UserRegistrationService(IUserRepository repository)
        {
            this.repository = repository;
        }

        public void RegisterUser(User user)
        {
            this.repository.Save(user);
        }
    }
}
