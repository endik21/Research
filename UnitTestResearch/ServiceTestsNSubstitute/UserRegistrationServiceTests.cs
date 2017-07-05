namespace ServiceTestsNSubstitute
{
    using DataAccess;

    using Domain;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NSubstitute;

    using Service;

    /// <summary>
    /// This is an example class for unit tests assesing specific methods invocation in a mock and stub case
    /// </summary>
    [TestClass]
    public class UserRegistrationServiceTestsFakeItEasy
    {
        /// <summary>
        /// example1: Mocking/tracking: Testing if method was called
        /// </summary>
        [TestMethod]
        public void Case1_RegisterUser_SavesTheUser()
        {
            // Arrage
            var userRepository = Substitute.For<IUserRepository>();
            
            // Act
            var registrationService = new UserRegistrationService(userRepository);
            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };
            registrationService.RegisterUser(user);

            // Assert
            userRepository.Received().Save(user);
        }

        /// <summary>
        /// example3: Stubing/controlling: Stubing method of mocked object
        /// </summary>
        [TestMethod]
        public void Case3_RegisterUser_SavesTheUser_WhenTheUserIsValid()
        {
            var userRepository = Substitute.For<IUserRepository>();
            var userValidator = Substitute.For<IUserValidator>();
            var userView = Substitute.For<IUserView>();
            userValidator.Validate(Arg.Any<User>()).Returns(true);
            
            var registrationService = new UserRegistrationServiceWithValidator(
                userView,
                userRepository,
                userValidator);

            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };

            registrationService.RegisterUser(user);

            userRepository.Received().Save(user);
        }

        /// <summary>
        /// example5: Match constraints i.e. for user parametric creation
        /// </summary>
        [TestMethod]
        public void Case5_RegisterUser_SavesTheUserFromParams_WhenTheUserIsValid()
        {
            var userRepository = Substitute.For<IUserRepository>();
            var userValidator = Substitute.For<IUserValidator>();
            userValidator.Validate(Arg.Any<User>()).Returns(true);

            var userRegistrationService = new UserRegistrationServiceWithValidator(
                userRepository,
                userValidator);

            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };

            userRegistrationService.RegisterUser(user.Id, user.FirstName, user.LastName);

            userRepository.Received()
                .Save(
                    Arg.Is<User>(u => user.Id == u.Id && user.FirstName == u.FirstName & u.LastName == user.LastName));
        }
    }
}