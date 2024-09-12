using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BedTrack.Application.Controllers;
using BedTrack.Application.NewDTO;
using BedTrack.Application.DTO;
using BedTrack.Domain.Interfaces;
using BedTrack.Domain.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BedTrackAPI.Tests
{
    public class BedControllerTest
    {
        private readonly Mock<IBedLogic> _mockBedLogic;
        private readonly BedController _controller;

        public BedControllerTest()
        {
            _mockBedLogic = new Mock<IBedLogic>();
            _controller = new BedController(_mockBedLogic.Object);
        }

        [Fact]
        public async Task Post_ShouldReturnOk_WhenBedIsValid()
        {
            // Arrange
            var newBed = new NewBedDTO {  };

            _mockBedLogic
                .Setup(logic => logic.CreateNewBed(It.IsAny<NewBedDTO>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(newBed);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Bed successfully created!", okResult.Value);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenBedIsNull()
        {
            // Act
            var result = await _controller.Post(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while creating an bed!", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithBeds_WhenBedsExist()
        {
            // Arrange
            var beds = new List<BedDTO> { new BedDTO { Id = 1 }, new BedDTO { Id = 2 } };
            _mockBedLogic.Setup(logic => logic.GetBeds()).ReturnsAsync(beds);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBeds = Assert.IsType<List<BedDTO>>(okResult.Value);
            Assert.Equal(2, returnedBeds.Count);
        }

        [Fact]
        public async Task Get_ShouldReturnOkWithBed_WhenBedExists()
        {
            // Arrange
            var bed = new BedDTO { Id = 1 };
            _mockBedLogic.Setup(logic => logic.GetBed(1)).ReturnsAsync(bed);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBed = Assert.IsType<BedDTO>(okResult.Value);
            Assert.Equal(1, returnedBed.Id);
        }

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenBedDoesNotExist()
        {
            // Arrange
            _mockBedLogic.Setup(logic => logic.GetBed(1)).ReturnsAsync((BedDTO)null);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Could not find an bed with ID = 1", notFoundResult.Value);
        }

        [Fact]
        public async Task Put_ShouldReturnOk_WhenBedIsUpdatedSuccessfully()
        {
            // Arrange
            var updatedBed = new NewBedDTO { /* Initialize with valid data */ };
            _mockBedLogic.Setup(logic => logic.UpdateBed(1, updatedBed)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(1, updatedBed);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully updated the bed with ID = 1", okResult.Value);
        }

        [Fact]
        public async Task Put_ShouldReturnBadRequest_WhenBedIsNull()
        {
            // Act
            var result = await _controller.Put(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while updating an bed!", badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenBedIsDeletedSuccessfully()
        {
            // Arrange
            var bed = new BedDTO { Id = 1 };
            _mockBedLogic.Setup(logic => logic.GetBed(1)).ReturnsAsync(bed);
            _mockBedLogic.Setup(logic => logic.DeleteBed(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully deleted the bed with ID = 1", okResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenBedDoesNotExist()
        {
            // Arrange
            _mockBedLogic.Setup(logic => logic.GetBed(1)).ReturnsAsync((BedDTO)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Could not find an bed with ID = 1", notFoundResult.Value);
        }
    }
}
