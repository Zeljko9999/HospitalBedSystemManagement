using BedTrack.Application.Controllers;
using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;
using BedTrack.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BedTrackAPI.Tests
{
    public class DepartmentControllerTest
    {
        private readonly Mock<IDepartmentLogic> _mockDepartmentLogic;
        private readonly DepartmentController _controller;

        public DepartmentControllerTest()
        {
            // Create a mock object of IDepartmentLogic
            _mockDepartmentLogic = new Mock<IDepartmentLogic>();

            // Initialize the controller with the mock object
            _controller = new DepartmentController(_mockDepartmentLogic.Object);
        }

        [Fact]
        public async Task Post_ValidDepartment_ReturnsOk()
        {
            // Arrange
            var newDepartment = new NewDepartmentDTO { };

            // Act
            var result = await _controller.Post(newDepartment);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Department successfully created!", okResult.Value);

            // Verify that CreateNewDepartment was called once
            _mockDepartmentLogic.Verify(x => x.CreateNewDepartment(newDepartment), Times.Once);
        }

        [Fact]
        public async Task Post_NullDepartment_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.Post(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while creating an department!", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAll_ReturnsDepartments()
        {
            // Arrange
            var departments = new List<DepartmentDTO>
            {
                new DepartmentDTO { }
            };

            _mockDepartmentLogic.Setup(x => x.GetDepartments()).ReturnsAsync(departments);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(departments, okResult.Value);
        }

        [Fact]
        public async Task Get_ExistingId_ReturnsDepartment()
        {
            // Arrange
            var department = new DepartmentDTO { /* Initialize properties as needed */ };

            _mockDepartmentLogic.Setup(x => x.GetDepartment(It.IsAny<int>())).ReturnsAsync(department);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(department, okResult.Value);
        }

        [Fact]
        public async Task Get_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockDepartmentLogic.Setup(x => x.GetDepartment(It.IsAny<int>())).ReturnsAsync((DepartmentDTO)null);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Could not find an department with ID = 1", notFoundResult.Value);
        }

        [Fact]
        public async Task Put_ValidDepartment_ReturnsOk()
        {
            // Arrange
            var updatedDepartment = new NewDepartmentDTO { };

            // Act
            var result = await _controller.Put(1, updatedDepartment);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully updated the department with ID = 1", okResult.Value);

            // Verify that UpdateDepartment was called once
            _mockDepartmentLogic.Verify(x => x.UpdateDepartment(1, updatedDepartment), Times.Once);
        }

        [Fact]
        public async Task Put_NullDepartment_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.Put(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while updating an department!", badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ExistingId_ReturnsOk()
        {
            // Arrange
            var department = new DepartmentDTO { };
            _mockDepartmentLogic.Setup(x => x.GetDepartment(It.IsAny<int>())).ReturnsAsync(department);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully deleted the department with ID = 1", okResult.Value);

            // Verify that DeleteDepartment was called once
            _mockDepartmentLogic.Verify(x => x.DeleteDepartment(1), Times.Once);
        }

        [Fact]
        public async Task Delete_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockDepartmentLogic.Setup(x => x.GetDepartment(It.IsAny<int>())).ReturnsAsync((DepartmentDTO)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Could not find an department with ID = 1", notFoundResult.Value);
        }
    }
}
