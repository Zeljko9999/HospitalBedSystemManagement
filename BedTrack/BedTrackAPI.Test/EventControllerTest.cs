using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BedTrack.Application.Controllers;
using BedTrack.Application.NewDTO;
using BedTrack.Application.DTO;
using BedTrack.Domain.Interfaces;
using System.Threading.Tasks;

namespace BedTrackAPI.Tests
{
    public class EventControllerTest
    {
        private readonly Mock<IEvenetLogic> _mockEventLogic;
        private readonly EventController _controller;

        public EventControllerTest()
        {
            _mockEventLogic = new Mock<IEvenetLogic>();
            _controller = new EventController(_mockEventLogic.Object);
        }

        [Fact]
        public async Task Post_ShouldReturnOk_WhenEventIsValid()
        {
            // Arrange
            var newEvent = new NewEventDTO { /* Initialize with valid data */ };

            _mockEventLogic
                .Setup(logic => logic.CreateNewEvent(It.IsAny<NewEventDTO>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(newEvent);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Event successfully created!", okResult.Value);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenEventIsNull()
        {
            // Act
            var result = await _controller.Post(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while creating an event!", badRequestResult.Value);
        }

        [Fact]
        public async Task Get_ShouldReturnOkWithEvent_WhenEventExists()
        {
            // Arrange
            var eventDto = new EventDTO { Id = 1 };
            _mockEventLogic.Setup(logic => logic.GetEvent(1)).ReturnsAsync(eventDto);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEvent = Assert.IsType<EventDTO>(okResult.Value);
            Assert.Equal(1, returnedEvent.Id);
        }

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            _mockEventLogic.Setup(logic => logic.GetEvent(1)).ReturnsAsync((EventDTO)null);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Could not find an event with ID = 1", notFoundResult.Value);
        }

        [Fact]
        public async Task Put_ShouldReturnOk_WhenEventIsUpdatedSuccessfully()
        {
            // Arrange
            var updatedEvent = new NewEventDTO { /* Initialize with valid data */ };
            _mockEventLogic.Setup(logic => logic.UpdateEvent(1, updatedEvent)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(1, updatedEvent);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully updated the event with ID = 1", okResult.Value);
        }

        [Fact]
        public async Task Put_ShouldReturnBadRequest_WhenEventIsNull()
        {
            // Act
            var result = await _controller.Put(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while updating an event!", badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenEventIsDeletedSuccessfully()
        {
            // Arrange
            var eventDto = new EventDTO { Id = 1 };
            _mockEventLogic.Setup(logic => logic.GetEvent(1)).ReturnsAsync(eventDto);
            _mockEventLogic.Setup(logic => logic.DeleteEvent(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully deleted the event with ID = 1", okResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            _mockEventLogic.Setup(logic => logic.GetEvent(1)).ReturnsAsync((EventDTO)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Could not find an event with ID = 1", notFoundResult.Value);
        }
    }
}
