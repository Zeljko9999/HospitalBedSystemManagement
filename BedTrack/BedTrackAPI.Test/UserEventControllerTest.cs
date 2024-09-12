using BedTrack.Application.Controllers;
using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;
using BedTrack.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BedTrackAPI.Tests
{
    public class UserEventControllerTest
    {
        private readonly Mock<IUserEventLogic> _mockUserEventLogic;
        private readonly UserEventController _controller;

        public UserEventControllerTest()
        {
            _mockUserEventLogic = new Mock<IUserEventLogic>();
            _controller = new UserEventController(_mockUserEventLogic.Object);
        }

        [Fact]
        public async Task Post_ShouldReturnOk_WhenUserEventIsCreated()
        {
            // Arrange
            var newUserEvent = new NewUserEventDTO { /* Initialize with valid data */ };
            _mockUserEventLogic.Setup(logic => logic.CreateNewUserEvent(newUserEvent))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(newUserEvent);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("UserEvent successfully created!", okResult.Value);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenUserEventIsNull()
        {
            // Act
            var result = await _controller.Post(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while creating an UserEvent!", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithUserEvents_WhenUserEventsExist()
        {
            // Arrange
            var userEvents = new List<EventDTO> { new EventDTO { /* Initialize with valid data */ } };
            _mockUserEventLogic.Setup(logic => logic.GetUserEvents(It.IsAny<int>()))
                .ReturnsAsync(userEvents);

            // Act
            var result = await _controller.GetAll(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUserEvents = Assert.IsType<List<EventDTO>>(okResult.Value);
            Assert.Single(returnedUserEvents);
        }

        [Fact]
        public async Task Get_ShouldReturnOkWithUserEvent_WhenUserEventExists()
        {
            // Arrange
            var userEvent = new EventDTO { /* Initialize with valid data */ };
            _mockUserEventLogic.Setup(logic => logic.GetUserEvent(It.IsAny<int>()))
                .ReturnsAsync(userEvent);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUserEvent = Assert.IsType<EventDTO>(okResult.Value);
        }

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenUserEventDoesNotExist()
        {
            // Arrange
            _mockUserEventLogic.Setup(logic => logic.GetUserEvent(It.IsAny<int>()))
                .ReturnsAsync((EventDTO?)null);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Could not find an event of user with ID 1", notFoundResult.Value);
        }

        [Fact]
        public async Task Put_ShouldReturnOk_WhenUserEventIsUpdatedSuccessfully()
        {
            // Arrange
            var updatedUserEvent = new NewUserEventDTO { /* Initialize with valid data */ };
            _mockUserEventLogic.Setup(logic => logic.UpdateUserEvent(It.IsAny<int>(), updatedUserEvent))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(1, updatedUserEvent);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully updated the UserEvent with ID = 1", okResult.Value);
        }

        [Fact]
        public async Task Put_ShouldReturnBadRequest_WhenUserEventIsNull()
        {
            // Act
            var result = await _controller.Put(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while updating an UserEvent!", badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenUserEventIsDeletedSuccessfully()
        {
            // Arrange
            var userEvent = new UserEventDTO { /* Initialize with valid data */ };
            _mockUserEventLogic.Setup(logic => logic.GetUserEventRow(It.IsAny<int>()))
                .ReturnsAsync(userEvent);
            _mockUserEventLogic.Setup(logic => logic.DeleteUserEvent(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully deleted the event with ID = 1", okResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenUserEventDoesNotExist()
        {
            // Arrange
            _mockUserEventLogic.Setup(logic => logic.GetUserEventRow(It.IsAny<int>()))
                .ReturnsAsync((UserEventDTO?)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Could not find an event with ID = 1", notFoundResult.Value);
        }
    }
}
