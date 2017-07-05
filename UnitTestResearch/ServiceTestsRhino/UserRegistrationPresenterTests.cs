namespace ServiceTestsRhino
{
    using DataAccess;

    using Domain;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rhino.Mocks;

    using Service;

    /// <summary>
    /// This class is an exaple unit test case when we check if properties were set in a specific way
    /// </summary>
    [TestClass]
    public class UserRegistrationPresenterTests
    {
        /// <summary>
        /// example2: Mocking/tracking: Testing if Property was set
        /// </summary>
        [TestMethod]
        public void Case2_RegisterUser_SavesTheUser_SetsSavedToTrue()
        {
            var mockUserRepository = MockRepository.GenerateMock<IUserRepository>();
            var mockUserView = MockRepository.GenerateMock<IUserView>();

            var userRegistrationService =
                new UserRegistrationPresenter(mockUserView, mockUserRepository);

            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };

            userRegistrationService.RegisterUser(user);
            mockUserView.AssertWasCalled(x => x.Saved = true);
        }

        /// <summary>
        /// example4: Stubing/controlling: Stubing property of mocked object
        /// </summary>
        [TestMethod]
        public void Case4_RegisterUser_SavesTheUser_SetsSavedToTrue_WhenRequiresSavingIsSetToTrueOnTheView()
        {
            var mockUserRepository = MockRepository.GenerateMock<IUserRepository>();
            var mockUserView = MockRepository.GenerateMock<IUserView>();
            mockUserView.Stub(x => x.Changed).Return(true);

            var registrationService =
                new UserRegistrationPresenterWithRequiresSaving(mockUserView, mockUserRepository);

            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };

            registrationService.RegisterUser(user);
            mockUserView.AssertWasCalled(x => x.Saved = true);
        }
    }
}
