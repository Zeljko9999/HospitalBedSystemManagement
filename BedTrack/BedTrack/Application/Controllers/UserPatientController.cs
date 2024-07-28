using BedTrack.Application.NewDTO;
using BedTrack.Domain.Interfaces;
using BedTrack.Domain.Logic;
using BedTrack.Domain.Models;
using FactoryApplication.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BedTrack.Application.Controllers
{
    [ErrorFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class UserPatientController : ControllerBase
    {
        private readonly IUserPatientLogic _userPatientLogic;

        public UserPatientController(IUserPatientLogic userPatientLogic)
        {
            this._userPatientLogic = userPatientLogic;
        }

        // Create an UserPatient object

        [HttpPost, Authorize]
        public async Task<IActionResult> Post([FromBody] NewUserPatientDTO userPatient)
        {

            if (userPatient != null)
            {
                await _userPatientLogic.CreateNewUserPatient(userPatient);
                return Ok("UserPatient successfully created!");
            }
            else
            {
                return BadRequest("Error while creating an UserPatient!");
            }
        }

        // Read Operation 1 - Get all patients of user with the specified ID

        [HttpGet("{userId}"), Authorize]
        public async Task<IActionResult> GetAllPatients(int userId)
        {
            var userPatients = await _userPatientLogic.GetPatientsForUser(userId);
            return Ok(userPatients);
        }

        // Read Operation 2 - Get all user of patient with the specified ID

        [HttpGet("{patientId}"), Authorize]
        public async Task<IActionResult> GetAllUsers(int patientId)
        {
            var userPatients = await _userPatientLogic.GetUsersForPatient(patientId);
            return Ok(userPatients);
        }

        // Update Operation - Update the UserPatient with the specified ID

        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] NewUserPatientDTO updatedUserPatient)
        {

            if (updatedUserPatient == null)
            {
                return BadRequest($"Error while updating an UserPatient!");
            }

            await _userPatientLogic.UpdateUserPatient(id, updatedUserPatient);

            return Ok($"Successfully updated the UserPatient with ID = {id}");
        }

        // Delete Operation - Delete the UserPatient with the specified ID

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userPatient = await _userPatientLogic.GetUserPatientRow(id);
            if (userPatient == null)
            {
                return NotFound($"Could not find an UserPatient with ID = {id}");
            }
            else
            {

                await _userPatientLogic.DeleteUserPatient(id);

                return Ok($"Successfully deleted the userPatient with ID = {id}");
            }
        }
    }
}
