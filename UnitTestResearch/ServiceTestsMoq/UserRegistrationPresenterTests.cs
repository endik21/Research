namespace ServiceTestsMoq
{
    using DataAccess;

    using Domain;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

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
            var mockRepository = new Mock<IUserRepository>();
            var mockView = new Mock<IUserView>();
            //var mockApplicationSettings = new Mock<IApplicationSettings>();   //mocking hierarchy of class
            //mockApplicationSettings.Setup(x => x.SystemConfiguration.AuditingInformation.WorkstationId).Rerurn(123);

            var registrationService =
                new UserRegistrationPresenter(mockView.Object, mockRepository.Object);

            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };

            registrationService.RegisterUser(user);

            mockView.VerifySet(x => x.Saved = true);
            //mockApplicationSettings.VerifyGet(x => x.SystemConfiguration.AuditingInformation.WorkstationId);  //verifying hierarchy of class
        }

        /// <summary>
        /// example4: Stubing/controlling: Stubing property of mocked object
        /// </summary>
        [TestMethod]
        public void Case4_RegisterUser_SavesTheUser_SetsSavedToTrue_WhenRequiresSavingIsSetToTrueOnTheView()
        {
            var mockRepository = new Mock<IUserRepository>();
            var mockView = new Mock<IUserView>();
            mockView.SetupProperty(x => x.Changed, true);

            // mockView.SetupAllProperties();         // case with many properties - Stub all
            // mockView.Object.Changed = true;        //  set specif property

            var registrationService =
                new UserRegistrationPresenterWithRequiresSaving(mockView.Object, mockRepository.Object);

            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };

            registrationService.RegisterUser(user);
            mockView.VerifySet(x => x.Saved = true);
        }
    }
}
