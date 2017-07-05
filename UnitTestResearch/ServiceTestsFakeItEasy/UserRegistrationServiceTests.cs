namespace ServiceTestsMoq
{
    using DataAccess;

    using Domain;

    using FakeItEasy;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var userRepository = A.Fake<IUserRepository>();
            
            // Act
            var registrationService = new UserRegistrationService(userRepository);
            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };
            registrationService.RegisterUser(user);

            // Assert
            A.CallTo(() => userRepository.Save(A<User>.Ignored)).MustHaveHappened();
        }

        /// <summary>
        /// example3: Stubing/controlling: Stubing method of mocked object
        /// </summary>
        [TestMethod]
        public void Case3_RegisterUser_SavesTheUser_WhenTheUserIsValid()
        {
            var userRepository = A.Fake<IUserRepository>();
            var userValidator = A.Fake<IUserValidator>();
            var userView = A.Fake<IUserView>();
            A.CallTo(() => userValidator.Validate(A<User>.Ignored)).Returns(true);

            var registrationService = new UserRegistrationServiceWithValidator(
                userView,
                userRepository,
                userValidator);

            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };

            registrationService.RegisterUser(user);

            A.CallTo(() => userRepository.Save(user)).MustHaveHappened();
        }

        /// <summary>
        /// example5: Match constraints i.e. for user parametric creation
        /// </summary>
        [TestMethod]
        public void Case5_RegisterUser_SavesTheUserFromParams_WhenTheUserIsValid()
        {
            var userRepository = A.Fake<IUserRepository>();
            var userValidator = A.Fake<IUserValidator>();
            A.CallTo(() => userValidator.Validate(A<User>.Ignored)).Returns(true);
            
            var userRegistrationService = new UserRegistrationServiceWithValidator(
                userRepository,
                userValidator);

            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };

            userRegistrationService.RegisterUser(user.Id, user.FirstName, user.LastName);

            A.CallTo(() => userRepository.Save(
                A<User>.That.Matches(u => u.FirstName == user.FirstName & u.LastName == user.LastName))).MustHaveHappened();
        }
    }
}