using BedTrack.Application.NewDTO;
using BedTrack.Domain.Interfaces;
using BedTrack.Domain.Logic;
using BedTrack.Domain.Models;
using FactoryApplication.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BedTrack.Application.Controllers
{
    [ErrorFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicDepartmentController : ControllerBase
    {
        private readonly IClinicDepartmentLogic _clinicDepartmentLogic;

        public ClinicDepartmentController(IClinicDepartmentLogic clinicDepartmentLogic)
        {
            this._clinicDepartmentLogic = clinicDepartmentLogic;
        }

        // Create an clinicDepartment object

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewClinicDepartmentDTO clinicDepartment)
        {

            if (clinicDepartment != null)
            {
                await _clinicDepartmentLogic.CreateNewClinicDepartment(clinicDepartment);
                return Ok("ClinicDepartment successfully created!");
            }
            else
            {
                return BadRequest("Error while creating an ClinicDepartment!");
            }
        }

        // Read Operation 1 - Get all departments of clinic with the specified ID

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAll(int id)
        {
            var clinicDepartments = await _clinicDepartmentLogic.GetClinicDepartments(id);
            return Ok(clinicDepartments);
        }

        // Read Operation 2 - Get the department of clinic with the specified ID

        [HttpGet("{clinicId}&{departmentId}")]
        public async Task<IActionResult> Get(int clinicId, int departmentId)
        {
            var clinicDepartment = await _clinicDepartmentLogic.GetClinicDepartment(clinicId, departmentId);

            if (clinicDepartment is null)
            {
                return NotFound($"Could not find an department with ID = {departmentId} on clinic with ID = {clinicId}");
            }
            else
            {
                return Ok(clinicDepartment);
            }
        }

        // Update Operation - Update the clinicDepartment with the specified ID

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NewClinicDepartmentDTO updatedClinicDepartment)
        {

            if (updatedClinicDepartment == null)
            {
                return BadRequest($"Error while updating an clinicDepartment!");
            }

            await _clinicDepartmentLogic.UpdateClinicDepartment(id, updatedClinicDepartment);

            return Ok($"Successfully updated the clinicDepartment with ID = {id}");
        }

        // Delete Operation - Delete the clinicDepartment with the specified ID

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var clinicDepartmentBed = await _clinicDepartmentLogic.GetClinicDepartmentRow(id);
            if (clinicDepartmentBed == null)
            {
                return NotFound($"Could not find an clinicDepartment with ID = {id}");
            }
            else
            {

                await _clinicDepartmentLogic.DeleteClinicDepartment(id);

                return Ok($"Successfully deleted the clinicDepartment with ID = {id}");
            }
        }
    }
}
