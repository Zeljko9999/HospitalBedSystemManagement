using BedTrack.Application.Controllers;
using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;
using BedTrack.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BedTrackAPI.Tests
{
    public class ClinicControllerTest
    {
        private readonly Mock<IClinicLogic> _mockClinicLogic;
        private readonly ClinicController _controller;

        public ClinicControllerTest()
        {
            // Create a mock object of IClinicLogic
            _mockClinicLogic = new Mock<IClinicLogic>();

            // Initialize the controller with the mock object
            _controller = new ClinicController(_mockClinicLogic.Object);
        }

        [Fact]
        public async Task Post_ValidClinic_ReturnsOk()
        {
            // Arrange
            var newClinic = new NewClinicDTO { };

            // Act
            var result = await _controller.Post(newClinic);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Clinic successfully created!", okResult.Value);

            // Verify that CreateNewClinic was called once
            _mockClinicLogic.Verify(x => x.CreateNewClinic(newClinic), Times.Once);
        }

        [Fact]
        public async Task Post_NullClinic_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.Post(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while creating an clinic!", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAll_ReturnsClinics()
        {
            // Arrange
            var clinics = new List<ClinicDTO>
            {
                new ClinicDTO {  }
            };

            // Setup mock to return the list of ClinicDTO when GetClinics is called
            _mockClinicLogic.Setup(x => x.GetClinics()).ReturnsAsync(clinics);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(clinics, okResult.Value);
        }


        [Fact]
        public async Task Get_ExistingClinic_ReturnsClinic()
        {
            // Arrange
            var clinicId = 1;
            var clinic = new ClinicDTO { };
            _mockClinicLogic.Setup(x => x.GetClinic(clinicId)).ReturnsAsync(clinic);

            // Act
            var result = await _controller.Get(clinicId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(clinic, okResult.Value);
        }

        [Fact]
        public async Task Get_NonExistingClinic_ReturnsNotFound()
        {
            // Arrange
            var clinicId = 1;
            _mockClinicLogic.Setup(x => x.GetClinic(clinicId)).ReturnsAsync((ClinicDTO)null);

            // Act
            var result = await _controller.Get(clinicId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Could not find an clinic with ID = {clinicId}", notFoundResult.Value);
        }

        [Fact]
        public async Task Delete_ExistingClinic_ReturnsOk()
        {
            // Arrange
            var clinicId = 1;
            var clinic = new ClinicDTO();
            _mockClinicLogic.Setup(x => x.GetClinic(clinicId)).ReturnsAsync(clinic);

            // Act
            var result = await _controller.Delete(clinicId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Successfully deleted the clinic with ID = {clinicId}", okResult.Value);
        }

        [Fact]
        public async Task Delete_NonExistingClinic_ReturnsNotFound()
        {
            // Arrange
            var clinicId = 1;
            _mockClinicLogic.Setup(x => x.GetClinic(clinicId)).ReturnsAsync((ClinicDTO)null);

            // Act
            var result = await _controller.Delete(clinicId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Could not find an clinic with ID = {clinicId}", notFoundResult.Value);
        }
    }
}
