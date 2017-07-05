namespace ServiceTestsMoq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using DataAccess;

    using Domain;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Service;

    /// <summary>
    /// This is an example class for unit tests assesing specific methods invocation in a mock and stub case
    /// </summary>
    [TestClass]
    public class UserRegistrationServiceTestsMoq
    {
        /// <summary>
        /// example1: Mocking/tracking: Testing if method was called
        /// </summary>
        [TestMethod]
        public void Case1_RegisterUser_SavesTheUser()
        {
            // Arrage
            var userRepository = new Mock<IUserRepository>();

            // userRepository.Setup(x => x.Save(It.IsAny<User>()));              // case for void function
            userRepository.Setup(x => x.Save(It.IsAny<User>())).Returns(true);  // case for return param assertion 
            // var user = new User();                                            // case for out param assertion
            // userRepository.Setup(x => x.Save(out user)).Returns(true);
            // userRepository.Setup(x => x.Save(It.IsAny<User>())).Returns(() => true).Callback(() => false); //case for multiple returns;
            userRepository.Setup(x => x.Save(It.Is<User>(u => u.FirstName == "John")))
                .Returns(true); // case for stubbing with a param
            userRepository.Setup(x => x.Save(It.Is<User>(u => u.FirstName != "John")))
                .Throws<ArgumentNullException>(); // case for throwing specific exception

            // Act
            var registrationService = new UserRegistrationService(userRepository.Object);
            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };
            registrationService.RegisterUser(user);

            // Assert
            userRepository.Verify();
            userRepository.Verify(x => x.Save(It.IsAny<User>()), Times.Exactly(1)); // asserting invocation times
            userRepository.Verify(x => x.Save(It.Is<User>(u => u.FirstName == "John")), Times.Exactly(1)); // assertng specific argument was used
        }

        /// <summary>
        /// example3: Stubing/controlling: Stubing method of mocked object
        /// </summary>
        [TestMethod]
        public void Case3_RegisterUser_SavesTheUser_WhenTheUserIsValid()
        {
            var userRepository = new Mock<IUserRepository>();

            var userValidator = new Mock<IUserValidator>();

            var mockUserView = new Mock<IUserView>();

            userValidator.Setup(x => x.Validate(It.IsAny<User>())).Returns(true);

            mockUserView.SetupProperty(x => x.Changed, true);

            var registrationService = new UserRegistrationServiceWithValidator(
                mockUserView.Object,
                userRepository.Object,
                userValidator.Object);

            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };

            registrationService.RegisterUser(user);

            userRepository.Verify(x => x.Save(user), Times.Exactly(1));
        }

        /// <summary>
        /// example5: Lambda match verification i.e. for user parametric creation
        /// </summary>
        [TestMethod]
        public void Case5_RegisterUser_SavesTheUserFromParams_WhenTheUserIsValid()
        {
            var userRepository = new Mock<IUserRepository>();
            var userValidator = new Mock<IUserValidator>();
            userValidator.Setup(x => x.Validate(It.IsAny<User>())).Returns(true);
            
            var userRegistrationService = new UserRegistrationServiceWithValidator(
                userRepository.Object,
                userValidator.Object);

            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };

            userRegistrationService.RegisterUser(user.Id, user.FirstName, user.LastName);

            userRepository.Verify(
                s => s.Save(It.Is<User>(x => x.FirstName == user.FirstName && x.LastName == user.LastName && x.Id == user.Id)));
        }

        /// <summary>
        /// example6: Lambda list verification i.e. for user list deletion
        /// </summary>
        [TestMethod]
        public void Case6_DeleteUsers_DeleteAllTheUsersPassedIn()
        {
            var user1 = new User { Id = 123, FirstName = "John1", LastName = "Doe" };
            var user2 = new User { Id = 456, FirstName = "John2", LastName = "Doe" };
            var user3 = new User { Id = 789, FirstName = "John3", LastName = "Doe" };

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.FindById(123)).Returns(user1);
            userRepository.Setup(x => x.FindById(456)).Returns(user2);
            userRepository.Setup(x => x.FindById(789)).Returns(user3);
            var userRegistrationService = new UserRegistrationServiceWithValidator(userRepository.Object, null);

            userRegistrationService.DeleteStudents(123, 456, 789);

            var temp = new List<User>() { user1, user2, user3 };

            userRepository.Verify(s => s.Delete(It.Is<List<User>>(l => l.All(v => temp.Contains(v)))));

            userRepository.Verify(s => s.Delete(It.Is<List<User>>(l => l.Count == temp.Count)));

            userRepository.Verify(s => s.Delete(It.Is<List<User>>(l => l.Contains(user1))));
        }

        /// <summary>
        /// example7: Lambda text verification i.e. for user finding by text
        /// </summary>
        [TestMethod]
        public void Case7_GetUserByLastName()
        {
            var userRepository = new Mock<IUserRepository>();
            var userRegistrationService = new UserRegistrationServiceWithValidator(userRepository.Object, null);

            userRegistrationService.GetUserByLastName("Jones");

            var user1 = new User { Id = 123, FirstName = "John1", LastName = "Doe" };

            userRepository.Verify(x => x.FindByLastName(It.Is<string>(s => s.Contains("one"))));

            userRepository.Verify(x => x.FindByLastName(It.Is<string>(s => s.EndsWith("nes"))));

            userRepository.Verify(x => x.FindByLastName(It.Is<string>(s => s.StartsWith("Jon"))));

            userRepository.Verify(x => x.FindByLastName(It.Is<string>(s => Regex.IsMatch(s, "Jon."))));
        }

    }
}