using BedTrack.Application.Controllers;
using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;
using BedTrack.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BedTrackAPI.Tests
{
    public class UserPatientControllerTest
    {
        private readonly Mock<IUserPatientLogic> _mockUserPatientLogic;
        private readonly UserPatientController _controller;

        public UserPatientControllerTest()
        {
            _mockUserPatientLogic = new Mock<IUserPatientLogic>();
            _controller = new UserPatientController(_mockUserPatientLogic.Object);
        }

        [Fact]
        public async Task Post_ShouldReturnOk_WhenUserPatientIsCreated()
        {
            // Arrange
            var newUserPatient = new NewUserPatientDTO { /* Initialize with valid data */ };
            _mockUserPatientLogic.Setup(logic => logic.CreateNewUserPatient(newUserPatient))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(newUserPatient);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("UserPatient successfully created!", okResult.Value);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenUserPatientIsNull()
        {
            // Act
            var result = await _controller.Post(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while creating an UserPatient!", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAllPatients_ShouldReturnOkWithUserPatients_WhenUserPatientsExist()
        {
            // Arrange
            var userPatients = new List<UserPatientDTO> { new UserPatientDTO { /* Initialize with valid data */ } };
            _mockUserPatientLogic.Setup(logic => logic.GetPatientsForUser(It.IsAny<int>()))
                .ReturnsAsync(userPatients);

            // Act
            var result = await _controller.GetAllPatients(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUserPatients = Assert.IsType<List<UserPatientDTO>>(okResult.Value);
            Assert.Single(returnedUserPatients);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnOkWithUserPatients_WhenUserPatientsExist()
        {
            // Arrange
            var userPatients = new List<UserPatientDTO> { new UserPatientDTO { /* Initialize with valid data */ } };
            _mockUserPatientLogic.Setup(logic => logic.GetUsersForPatient(It.IsAny<int>()))
                .ReturnsAsync(userPatients);

            // Act
            var result = await _controller.GetAllUsers(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUserPatients = Assert.IsType<List<UserPatientDTO>>(okResult.Value);
            Assert.Single(returnedUserPatients);
        }

        [Fact]
        public async Task Put_ShouldReturnOk_WhenUserPatientIsUpdatedSuccessfully()
        {
            // Arrange
            var updatedUserPatient = new NewUserPatientDTO { /* Initialize with valid data */ };
            _mockUserPatientLogic.Setup(logic => logic.UpdateUserPatient(It.IsAny<int>(), updatedUserPatient))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(1, updatedUserPatient);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully updated the UserPatient with ID = 1", okResult.Value);
        }

        [Fact]
        public async Task Put_ShouldReturnBadRequest_WhenUserPatientIsNull()
        {
            // Act
            var result = await _controller.Put(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while updating an UserPatient!", badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenUserPatientIsDeletedSuccessfully()
        {
            // Arrange
            var userPatient = new UserPatientDTO { /* Initialize with valid data */ };
            _mockUserPatientLogic.Setup(logic => logic.GetUserPatientRow(It.IsAny<int>()))
                .ReturnsAsync(userPatient);
            _mockUserPatientLogic.Setup(logic => logic.DeleteUserPatient(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully deleted the userPatient with ID = 1", okResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenUserPatientDoesNotExist()
        {
            // Arrange
            _mockUserPatientLogic.Setup(logic => logic.GetUserPatientRow(It.IsAny<int>()))
                .ReturnsAsync((UserPatientDTO?)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Could not find an UserPatient with ID = 1", notFoundResult.Value);
        }
    }
}
