using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BedTrack.Application.Controllers;
using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;
using BedTrack.Domain.Interfaces;
using System.Threading.Tasks;

namespace BedTrackAPI.Tests
{
    public class PatientControllerTest
    {
        private readonly Mock<IPatientLogic> _mockPatientLogic;
        private readonly PatientController _controller;

        public PatientControllerTest()
        {
            _mockPatientLogic = new Mock<IPatientLogic>();
            _controller = new PatientController(_mockPatientLogic.Object);
        }

        [Fact]
        public async Task Post_ShouldReturnOk_WhenPatientIsValid()
        {
            // Arrange
            var newPatient = new NewPatientDTO { /* Initialize with valid data */ };

            _mockPatientLogic
                .Setup(logic => logic.CreateNewPatient(It.IsAny<NewPatientDTO>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(newPatient);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Patient successfully created!", okResult.Value);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenPatientIsNull()
        {
            // Act
            var result = await _controller.Post(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while creating an patient!", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithPatients_WhenRoleIsBoss()
        {
            // Arrange
            var patients = new List<PatientDTO> { new PatientDTO { Id = 1 } };
            _mockPatientLogic.Setup(logic => logic.GetAllPatients()).ReturnsAsync(patients);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPatients = Assert.IsType<List<PatientDTO>>(okResult.Value);
            Assert.Single(returnedPatients);
            Assert.Equal(1, returnedPatients[0].Id);
        }

        [Fact]
        public async Task Get_ShouldReturnOkWithPatient_WhenPatientExists()
        {
            // Arrange
            var patientDto = new PatientDTO { Id = 1 };
            _mockPatientLogic.Setup(logic => logic.GetPatient(1)).ReturnsAsync(patientDto);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPatient = Assert.IsType<PatientDTO>(okResult.Value);
            Assert.Equal(1, returnedPatient.Id);
        }

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenPatientDoesNotExist()
        {
            // Arrange
            _mockPatientLogic.Setup(logic => logic.GetPatient(1)).ReturnsAsync((PatientDTO)null);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Could not find an patient with ID = 1", notFoundResult.Value);
        }

        [Fact]
        public async Task GetPatientsWithoutBeds_ShouldReturnOkWithPatients_WhenPatientsExist()
        {
            // Arrange
            var patients = new List<PatientDTO> { new PatientDTO { Id = 1 } };
            _mockPatientLogic.Setup(logic => logic.GetPatientsWithoutBed()).ReturnsAsync(patients);

            // Act
            var result = await _controller.GetPatientsWithoutBeds();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPatients = Assert.IsType<List<PatientDTO>>(okResult.Value);
            Assert.Single(returnedPatients);
            Assert.Equal(1, returnedPatients[0].Id);
        }

        [Fact]
        public async Task Put_ShouldReturnOk_WhenPatientIsUpdatedSuccessfully()
        {
            // Arrange
            var updatedPatient = new NewPatientDTO { /* Initialize with valid data */ };
            _mockPatientLogic.Setup(logic => logic.UpdatePatient(1, updatedPatient)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(1, updatedPatient);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully updated the patient with ID = 1", okResult.Value);
        }

        [Fact]
        public async Task Put_ShouldReturnBadRequest_WhenPatientIsNull()
        {
            // Act
            var result = await _controller.Put(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error while updating an patient!", badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenPatientIsDeletedSuccessfully()
        {
            // Arrange
            var patientDto = new PatientDTO { Id = 1 };
            _mockPatientLogic.Setup(logic => logic.GetPatient(1)).ReturnsAsync(patientDto);
            _mockPatientLogic.Setup(logic => logic.DeletePatient(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully deleted the patient with ID = 1", okResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenPatientDoesNotExist()
        {
            // Arrange
            _mockPatientLogic.Setup(logic => logic.GetPatient(1)).ReturnsAsync((PatientDTO)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Could not find an patient with ID = 1", notFoundResult.Value);
        }
    }
}
