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
    public class ClinicDepartmentBedControllerTest
    {
        private readonly Mock<IClinicDepartmentBedLogic> _mockClinicDepartmentBedLogic;
        private readonly ClinicDepartmentBedController _controller;

        public ClinicDepartmentBedControllerTest()
        {
            _mockClinicDepartmentBedLogic = new Mock<IClinicDepartmentBedLogic>();
            _controller = new ClinicDepartmentBedController(_mockClinicDepartmentBedLogic.Object);
        }

        [Fact]
        public async Task Post_ShouldReturnOk_WhenClinicDepartmentBedIsValid()
        {
            // Arrange
            var newClinicDepartmentBed = new NewClinicDepartmentBedDTO { /* Initialize with valid data */ };

            _mockClinicDepartmentBedLogic
                .Setup(logic => logic.CreateNewClinicDepartmentBed(It.IsAny<NewClinicDepartmentBedDTO>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(newClinicDepartmentBed);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("ClinicDepartmentBed successfully created!", okResult.Value);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenClinicDepartmentBedIsNull()
        {
            // Act
            var result = await _controller.Post(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while creating an ClinicDepartmentBed!", badRequestResult.Value);
        }

        [Fact]
        public async Task Get_ShouldReturnOkWithClinicDepartmentBed_WhenClinicDepartmentBedExists()
        {
            // Arrange
            var clinicDepartmentBed = new ClinicDepartmentBedDTO { Id = 1 };
            _mockClinicDepartmentBedLogic.Setup(logic => logic.GetClinicDepartmentBedRow(1)).ReturnsAsync(clinicDepartmentBed);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedClinicDepartmentBed = Assert.IsType<ClinicDepartmentBedDTO>(okResult.Value);
            Assert.Equal(1, returnedClinicDepartmentBed.Id);
        }


        [Fact]
        public async Task GetAllBeds_ShouldReturnOkWithClinicDepartmentBeds_WhenBedsExist()
        {
            // Arrange
            var clinicDepartmentBeds = new List<ClinicDepartmentBedDTO> { new ClinicDepartmentBedDTO { Id = 1 }, new ClinicDepartmentBedDTO { Id = 2 } };
            _mockClinicDepartmentBedLogic.Setup(logic => logic.GetBedsForClinicDepartment(1, 1)).ReturnsAsync(clinicDepartmentBeds);

            // Act
            var result = await _controller.GetAllBeds(1, 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedClinicDepartmentBeds = Assert.IsType<List<ClinicDepartmentBedDTO>>(okResult.Value);
            Assert.Equal(2, returnedClinicDepartmentBeds.Count);
        }

        [Fact]
        public async Task Put_ShouldReturnOk_WhenClinicDepartmentBedIsUpdatedSuccessfully()
        {
            // Arrange
            var updatedClinicDepartmentBed = new NewClinicDepartmentBedDTO { /* Initialize with valid data */ };
            _mockClinicDepartmentBedLogic.Setup(logic => logic.UpdateClinicDepartmentBed(1, updatedClinicDepartmentBed)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(1, updatedClinicDepartmentBed);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully updated the clinicDepartmentBed with ID = 1", okResult.Value);
        }

        [Fact]
        public async Task Put_ShouldReturnBadRequest_WhenClinicDepartmentBedIsNull()
        {
            // Act
            var result = await _controller.Put(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while updating an clinicDepartmentBed!", badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenClinicDepartmentBedIsDeletedSuccessfully()
        {
            // Arrange
            var clinicDepartmentBed = new ClinicDepartmentBedDTO { Id = 1 };
            _mockClinicDepartmentBedLogic.Setup(logic => logic.GetClinicDepartmentBedRow(1)).ReturnsAsync(clinicDepartmentBed);
            _mockClinicDepartmentBedLogic.Setup(logic => logic.DeleteClinicDepartmentBed(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully deleted the clinicDepartmentBed with ID = 1", okResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenClinicDepartmentBedDoesNotExist()
        {
            // Arrange
            _mockClinicDepartmentBedLogic.Setup(logic => logic.GetClinicDepartmentBedRow(1)).ReturnsAsync((ClinicDepartmentBedDTO)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Could not find an clinicDepartmentBed with ID = 1", notFoundResult.Value);
        }
    }
}
