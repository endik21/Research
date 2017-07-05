﻿namespace ServiceTestsNSubstitute
{
    using DataAccess;

    using Domain;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NSubstitute;

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
            var mockUserRepository = Substitute.For<IUserRepository>();
            var mockUserView = Substitute.For<IUserView>();

            var userRegistrationService =
                new UserRegistrationPresenter(mockUserView, mockUserRepository);

            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };

            userRegistrationService.RegisterUser(user);

            Assert.IsTrue(mockUserView.Saved);
        }

        /// <summary>
        /// example4: Stubing/controlling: Stubing property of mocked object
        /// </summary>
        [TestMethod]
        public void Case4_RegisterUser_SavesTheUser_SetsSavedToTrue_WhenRequiresSavingIsSetToTrueOnTheView()
        {
            var mockUserRepository = Substitute.For<IUserRepository>();
            var mockUserView = Substitute.For<IUserView>();

            mockUserView.Changed.Returns(true);
            
            var registrationService =
                new UserRegistrationPresenterWithRequiresSaving(mockUserView, mockUserRepository);

            var user = new User { Id = 123, FirstName = "John", LastName = "Doe" };

            registrationService.RegisterUser(user);
            Assert.IsTrue(mockUserView.Saved);
        }
    }
}
