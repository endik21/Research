namespace Service
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using DataAccess;

    using Domain;

    public class UserRegistrationServiceWithValidator
    {
        private readonly IUserRepository userRepository;

        private readonly IUserValidator userValidator;

        private readonly IUserView userView;

        public UserRegistrationServiceWithValidator(IUserView userView, IUserRepository userRepository, IUserValidator userValidator)
        {
            this.userRepository = userRepository;
            this.userView = userView;
            this.userValidator = userValidator;
        }

        public void RegisterUser(User user)
        {
            bool isUserValid = userValidator.Validate(user);
            if (isUserValid)
            {
                userRepository.Save(user);
            }
            else
            {
                
                throw new ArgumentException("Invalid User", "user");
            }

            if (user == null)
            {
                userView.Saved = false;
            }
        }

        public UserRegistrationServiceWithValidator(IUserRepository repository, IUserValidator userValidator)
        {
            this.userRepository = repository;
            this.userValidator = userValidator;
        }

        public void RegisterUser(int userId, string firstName, string lastName)
        {
            var user = new User()
                           {
                               Id = userId,
                               FirstName = firstName,
                               LastName = lastName
                           };

            bool isUserValid = userValidator.Validate(user);
            if (isUserValid)
            {
                userRepository.Save(user);
            }
            else
            {
                throw new ArgumentException("Invalid user");
            }
        }

        public void DeleteStudents(params int[] usersIds)
        {
            var users = new List<User>();
            foreach (var userId in usersIds)
            {
                users.Add(userRepository.FindById(userId));
            }

            userRepository.Delete(users);
        }

        public User GetUserByLastName(string lastName)
        {
            return userRepository.FindByLastName(lastName);
        } 
    }
}
