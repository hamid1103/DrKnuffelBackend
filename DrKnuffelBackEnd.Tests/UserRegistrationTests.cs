using Microsoft.AspNetCore.Mvc;
using Moq;
using DrKnuffelBackEnd.Controllers;
using DrKnuffelBackEnd.Models;
using DrKnuffelBackEnd.Repositories;
using DrKnuffelBackEnd.Services;
using Microsoft.EntityFrameworkCore.Storage;
using DrKnuffelBackEnd.Repositories.UserData;

namespace DrKnuffelBackEnd.Tests
{
    [TestClass]
    public sealed class UserRegistrationTests
    {
        private ExtraUserDataController controller;
        private Mock<IExtraUserData> userDataRepository;
        private Mock<IAuthenticationService> authenticationService;
        private string userId;

        [TestInitialize]
        public void Setup()
        {
            userDataRepository = new Mock<IExtraUserData>();
            authenticationService = new Mock<IAuthenticationService>();

            userId = Guid.NewGuid().ToString();

            authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(userId);

            controller = new ExtraUserDataController(userDataRepository.Object, authenticationService.Object);
        }

        [TestMethod]
        public async Task GetAsync_NoUser_ReturnsUnauthorized()
        {
            // Arrange
            authenticationService
                .Setup(x => x.GetCurrentAuthenticatedUserId())
                .Returns((string)null);

            // Act
            var response = await controller.GetAsync();

            // Assert
            Assert.IsInstanceOfType(response.Result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public async Task InsertAsync_ValidUserData_ReturnsOk()
        {
            // Arrange
            var userData = new UserData
            {
                DoctorName = "Dr. Smith",
                UserAge = 30
            };

            userDataRepository
                .Setup(x => x.AddAsync(It.IsAny<UserData>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await controller.InsertAsync(userData);

            // Assert
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));

            var okResult = response as OkObjectResult;
            var returnedData = okResult.Value as UserData;

            Assert.IsNotNull(returnedData.Id);
            Assert.AreEqual(userId, returnedData.UserId);
        }

        [TestMethod]
        public async Task GetAsync_WithUser_ReturnsUserData()
        {
            // Arrange
            var data = new UserData
            {
                DoctorName = "Dr. A",
                UserAge = 25
            };

            userDataRepository
                .Setup(x => x.GetAsyncByUserId(userId))
                .Returns(Task.FromResult(data));

            // Act
            var response = await controller.GetAsync();

            // Assert
            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));

            var okResult = response.Result as OkObjectResult;
            var result = okResult.Value as UserData;

            Assert.IsNotNull(result);
            Assert.AreEqual("Dr. A", result.DoctorName);

        }
    }
}