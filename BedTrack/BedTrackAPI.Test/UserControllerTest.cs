using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BedTrack.Application.Controllers;
using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;
using BedTrack.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedTrackAPI.Tests
{
    public class UserControllerTest
    {
        private readonly Mock<IBasicUserLogic> _mockUserLogic;
        private readonly UserController _controller;

        public UserControllerTest()
        {
            _mockUserLogic = new Mock<IBasicUserLogic>();
            _controller = new UserController(_mockUserLogic.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithUsers_WhenUsersExist()
        {
            // Arrange
            var users = new List<UserDTO> { new UserDTO { Id = 1 } };
            _mockUserLogic.Setup(logic => logic.GetUsers()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsType<List<UserDTO>>(okResult.Value);
            Assert.Single(returnedUsers);
            Assert.Equal(1, returnedUsers[0].Id);
        }

        [Fact]
        public async Task Get_ShouldReturnOkWithUser_WhenUserExists()
        {
            // Arrange
            var userDto = new UserDTO { Id = 1 };
            _mockUserLogic.Setup(logic => logic.GetUser(1)).ReturnsAsync(userDto);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(1, returnedUser.Id);
        }

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserLogic.Setup(logic => logic.GetUser(1)).ReturnsAsync((UserDTO)null);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Could not find an user with ID = 1", notFoundResult.Value);
        }

        [Fact]
        public async Task GetUserByEmail_ShouldReturnOkWithUser_WhenUserExists()
        {
            // Arrange
            var userDto = new UserDTO { Id = 1 };
            _mockUserLogic.Setup(logic => logic.GetUserByEmail("test@example.com")).ReturnsAsync(userDto);

            // Act
            var result = await _controller.GetUserByEmail("test@example.com");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(1, returnedUser.Id);
        }

        [Fact]
        public async Task GetUserByEmail_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserLogic.Setup(logic => logic.GetUserByEmail("test@example.com")).ReturnsAsync((UserDTO)null);

            // Act
            var result = await _controller.GetUserByEmail("test@example.com");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Could not find an user with email = test@example.com", notFoundResult.Value);
        }

        [Fact]
        public async Task Put_ShouldReturnOk_WhenUserIsUpdatedSuccessfully()
        {
            // Arrange
            var updatedUser = new NewUserDTO { /* Initialize with valid data */ };
            _mockUserLogic.Setup(logic => logic.UpdateUser(1, updatedUser)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(1, updatedUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully updated the user with ID = 1", okResult.Value);
        }

        [Fact]
        public async Task Put_ShouldReturnBadRequest_WhenUserIsNull()
        {
            // Act
            var result = await _controller.Put(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while updating an user!", badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenUserIsDeletedSuccessfully()
        {
            // Arrange
            var userDto = new UserDTO { Id = 1 };
            _mockUserLogic.Setup(logic => logic.GetUser(1)).ReturnsAsync(userDto);
            _mockUserLogic.Setup(logic => logic.DeleteUser(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully deleted the user with ID = 1", okResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserLogic.Setup(logic => logic.GetUser(1)).ReturnsAsync((UserDTO)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Could not find an user with ID = 1", notFoundResult.Value);
        }
 
    }
}
