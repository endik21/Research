namespace ServiceTestsRhino
{
    using System.Collections.Generic;

    using DataAccess;

    using Domain;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rhino.Mocks;

    using Service;

    /// <summary>
    /// This is an example class for unit tests assesing specific methods invocation in a mock and stub case
    /// </summary>
    [TestClass]
    public class UserRegistrationServiceTests
    {
        /// <summary>
        /// example1: Mocking/tracking: Testing if method was called
        /// </summary>
        [TestMethod]
        public void Case1_RegisterUser_SavesTheUser()
        {
            var userRepository = MockRepository.GenerateMock<IUserRepository>();
            var userRegistrationService = new UserRegistrationService(userRepository);
            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };

            userRegistrationService.RegisterUser(user);
            userRepository.AssertWasCalled(x => x.Save(user));
        }

        /// <summary>
        /// example3: Stubing/controlling: Stubing method of mocked object
        /// </summary>
        [TestMethod]
        public void Case3_RegisterUser_SavesTheUser_WhenTheUserIsValid()
        {
            var userRepository = MockRepository.GenerateMock<IUserRepository>();
            var userValidator = MockRepository.GenerateMock<IUserValidator>();
            var userView = MockRepository.GenerateMock<IUserView>();
            userValidator.Stub(x => x.Validate(Arg<User>.Is.Anything)).Return(true);

            var registrationService = new UserRegistrationServiceWithValidator(
                userView,
                userRepository,
                userValidator);

            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };

            registrationService.RegisterUser(user);

            userRepository.AssertWasCalled(x => x.Save(user));
        }

        /// <summary>
        /// example5: Match constraints i.e. for user parametric creation
        /// </summary>
        [TestMethod]
        public void Case5_RegisterUser_SavesTheUserFromParams_WhenTheUserIsValid()
        {
            var userRepository = MockRepository.GenerateMock<IUserRepository>();
            var userValidator = MockRepository.GenerateMock<IUserValidator>();
            userValidator.Stub(x => x.Validate(Arg<User>.Is.Anything)).Return(true);

            var userRegistrationService = new UserRegistrationServiceWithValidator(
                userRepository,
                userValidator);

            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };

            userRegistrationService.RegisterUser(user.Id, user.FirstName, user.LastName);

            userRepository.AssertWasCalled(
                s => s.Save(
                    Arg<User>.Matches(x => x.Id == user.Id && x.FirstName == user.FirstName && x.LastName == user.LastName)));
        }

        /// <summary>
        /// example6: List Constraints i.e. for user list deletion
        /// </summary>
        [TestMethod]
        public void Case6_DeleteUsers_DeleteAllTheUsersPassedIn()
        {
            var user1 = new User { Id = 123, FirstName = "John1", LastName = "Doe" };
            var user2 = new User { Id = 456, FirstName = "John2", LastName = "Doe" };
            var user3 = new User { Id = 789, FirstName = "John3", LastName = "Doe" };

            var userRepository = MockRepository.GenerateMock<IUserRepository>();
            userRepository.Stub(x => x.FindById(123)).Return(user1);
            userRepository.Stub(x => x.FindById(456)).Return(user2);
            userRepository.Stub(x => x.FindById(789)).Return(user3);
            var userRegistrationService = new UserRegistrationServiceWithValidator(userRepository, null);

            userRegistrationService.DeleteStudents(123, 456, 789);

            userRepository.AssertWasCalled(
                s => s.Delete(Arg<List<User>>.List.ContainsAll(new List<User>() { user1, user2, user3 })));

            userRepository.AssertWasCalled(
                s => s.Delete(Arg<List<User>>.List.Equal(new List<User>() { user1, user2, user3 })));

            // equivalent for the first two
            userRepository.AssertWasCalled(s => s.Delete(new List<User>() { user1, user2, user3 }));

            userRepository.AssertWasCalled(
                s => s.Delete(Arg<List<User>>.List.Count(Rhino.Mocks.Constraints.Is.Equal(3))));

            userRepository.AssertWasCalled(
                s => s.Delete(Arg<List<User>>.List.IsIn(user1)));
        }

        /// <summary>
        /// example7: Text Constraints i.e. for user finding by text
        /// </summary>
        [TestMethod]
        public void Case7_GetUserByLastName()
        {
            var userRepository = MockRepository.GenerateMock<IUserRepository>();
            var userRegistrationService = new UserRegistrationServiceWithValidator(userRepository, null);

            userRegistrationService.GetUserByLastName("Jones");

            var user1 = new User { Id = 123, FirstName = "John1", LastName = "Doe" };

            userRepository.AssertWasCalled(
                s => s.FindByLastName(Arg<string>.Matches(Rhino.Mocks.Constraints.Text.Contains("one"))));

            userRepository.AssertWasCalled(
                s => s.FindByLastName(Arg<string>.Matches(Rhino.Mocks.Constraints.Text.EndsWith("nes"))));

            userRepository.AssertWasCalled(
                s => s.FindByLastName(Arg<string>.Matches(Rhino.Mocks.Constraints.Text.Contains("Jon"))));

            userRepository.AssertWasCalled(
                s => s.FindByLastName(Arg<string>.Matches(Rhino.Mocks.Constraints.Text.Like("Jone.*"))));
        }
    }
}