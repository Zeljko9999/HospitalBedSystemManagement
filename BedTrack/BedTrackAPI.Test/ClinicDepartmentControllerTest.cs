using BedTrack.Application.Controllers;
using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;
using BedTrack.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BedTrackAPI.Tests
{
    public class ClinicDepartmentControllerTest
    {
        private readonly Mock<IClinicDepartmentLogic> _mockClinicDepartmentLogic;
        private readonly ClinicDepartmentController _controller;

        public ClinicDepartmentControllerTest()
        {
            // Create a mock object of IClinicDepartmentLogic
            _mockClinicDepartmentLogic = new Mock<IClinicDepartmentLogic>();

            // Initialize the controller with the mock object
            _controller = new ClinicDepartmentController(_mockClinicDepartmentLogic.Object);
        }

        [Fact]
        public async Task Post_ValidClinicDepartment_ReturnsOk()
        {
            // Arrange
            var newClinicDepartment = new NewClinicDepartmentDTO {  };

            // Act
            var result = await _controller.Post(newClinicDepartment);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("ClinicDepartment successfully created!", okResult.Value);

            // Verify that CreateNewClinicDepartment was called once
            _mockClinicDepartmentLogic.Verify(x => x.CreateNewClinicDepartment(newClinicDepartment), Times.Once);
        }

        [Fact]
        public async Task Post_NullClinicDepartment_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.Post(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while creating an ClinicDepartment!", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAll_ReturnsClinicDepartments()
        {
            // Arrange
            var clinicDepartments = new List<ClinicDepartmentDTO>
            {
                new ClinicDepartmentDTO {  }
            };

            _mockClinicDepartmentLogic.Setup(x => x.GetClinicDepartments(It.IsAny<int>())).ReturnsAsync(clinicDepartments);

            // Act
            var result = await _controller.GetAll(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(clinicDepartments, okResult.Value);
        }

        [Fact]
        public async Task Get_ExistingClinicAndDepartmentId_ReturnsClinicDepartment()
        {
            // Arrange
            var clinicDepartment = new ClinicDepartmentDTO {  };

            _mockClinicDepartmentLogic.Setup(x => x.GetClinicDepartment(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(clinicDepartment);

            // Act
            var result = await _controller.Get(1, 2);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(clinicDepartment, okResult.Value);
        }

        [Fact]
        public async Task Get_NonExistingClinicAndDepartmentId_ReturnsNotFound()
        {
            // Arrange
            _mockClinicDepartmentLogic.Setup(x => x.GetClinicDepartment(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((ClinicDepartmentDTO)null);

            // Act
            var result = await _controller.Get(1, 2);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Could not find an department with ID = 2 on clinic with ID = 1", notFoundResult.Value);
        }

        [Fact]
        public async Task Put_ValidClinicDepartment_ReturnsOk()
        {
            // Arrange
            var updatedClinicDepartment = new NewClinicDepartmentDTO {  };

            // Act
            var result = await _controller.Put(1, updatedClinicDepartment);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully updated the clinicDepartment with ID = 1", okResult.Value);

            // Verify that UpdateClinicDepartment was called once
            _mockClinicDepartmentLogic.Verify(x => x.UpdateClinicDepartment(1, updatedClinicDepartment), Times.Once);
        }

        [Fact]
        public async Task Put_NullClinicDepartment_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.Put(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while updating an clinicDepartment!", badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ExistingId_ReturnsOk()
        {
            // Arrange
            var clinicDepartment = new ClinicDepartmentDTO {  };
            _mockClinicDepartmentLogic.Setup(x => x.GetClinicDepartmentRow(It.IsAny<int>())).ReturnsAsync(clinicDepartment);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully deleted the clinicDepartment with ID = 1", okResult.Value);

            // Verify that DeleteClinicDepartment was called once
            _mockClinicDepartmentLogic.Verify(x => x.DeleteClinicDepartment(1), Times.Once);
        }

        [Fact]
        public async Task Delete_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockClinicDepartmentLogic.Setup(x => x.GetClinicDepartmentRow(It.IsAny<int>())).ReturnsAsync((ClinicDepartmentDTO)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Could not find an clinicDepartment with ID = 1", notFoundResult.Value);
        }
    }
}
