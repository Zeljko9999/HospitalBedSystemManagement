using BedTrack.Application.DTO;
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
    public class PatientController : ControllerBase
    {
        private readonly IPatientLogic _patientLogic;

        public PatientController(IPatientLogic patientLogic)
        {
            this._patientLogic = patientLogic;
        }

        // Create an patient object

        [HttpPost, Authorize]
        public async Task<IActionResult> Post([FromBody] NewPatientDTO patient)
        {

            if (patient != null)
            {
                await _patientLogic.CreateNewPatient(patient);
                return Ok("Patient successfully created!");
            }
            else
            {
                return BadRequest("Error while creating an patient!");
            }
        }

        // Read Operation 3 - Get all patients

        [HttpGet, Authorize(Roles = "Boss")]
        public async Task<IActionResult> Get()
        {
            var patients = await _patientLogic.GetAllPatients();
            return Ok(patients);
        }

        // Read Operation 2 - Get the patient with the specified ID

        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var patient = await _patientLogic.GetPatient(id);

            if (patient is null)
            {
                return NotFound($"Could not find an patient with ID = {id}");
            }
            else
            {
                return Ok(patient);
            }
        }

        // Read Operation 3 - Get all patients without bed
        [HttpGet("without-beds"), Authorize]
        public async Task<IActionResult> GetPatientsWithoutBeds()
        {
            var patients = await _patientLogic.GetPatientsWithoutBed();
            return Ok(patients);
        }

        // Update Operation - Update the patient with the specified ID

        [HttpPatch("edit/{id}"), Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] NewPatientDTO updatedPatient)
        {

            if (updatedPatient == null)
            {
                return BadRequest($"Error while updating an patient!");
            }

            await _patientLogic.UpdatePatient(id, updatedPatient);

            return Ok($"Successfully updated the patient with ID = {id}");
        }

        // Delete Operation - Delete the patient with the specified ID

        [HttpDelete("{id}"), Authorize(Roles = "Clerk")]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _patientLogic.GetPatient(id);
            if (patient == null)
            {
                return NotFound($"Could not find an patient with ID = {id}");
            }
            else
            {

                await _patientLogic.DeletePatient(id);

                return Ok($"Successfully deleted the patient with ID = {id}");
            }
        }
    }
}
