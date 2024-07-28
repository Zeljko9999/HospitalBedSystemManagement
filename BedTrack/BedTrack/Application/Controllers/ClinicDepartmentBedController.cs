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
    public class ClinicDepartmentBedController : ControllerBase
    {
        private readonly IClinicDepartmentBedLogic _clinicDepartmentBedLogic;

        public ClinicDepartmentBedController(IClinicDepartmentBedLogic clinicDepartmentBedLogic)
        {
            this._clinicDepartmentBedLogic = clinicDepartmentBedLogic;
        }

        // Create an clinicDepartmentBed object

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] NewClinicDepartmentBedDTO clinicDepartmentBed)
        {

            if (clinicDepartmentBed != null)
            {
                await _clinicDepartmentBedLogic.CreateNewClinicDepartmentBed(clinicDepartmentBed);
                return Ok("ClinicDepartmentBed successfully created!");
            }
            else
            {
                return BadRequest("Error while creating an ClinicDepartmentBed!");
            }
        }

        // Read Operation 1 - Get clinicDepartmentBed with the specified ID

        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var clinicDepartmentBed = await _clinicDepartmentBedLogic.GetClinicDepartmentBedRow(id);
            return Ok(clinicDepartmentBed);
        }

        // Read Operation 2 - Get all beds for cilinicDepartment with the specified clinicId and departmentId

        [HttpGet("/AllBeds/{clinicId}&{departmentId}"), Authorize]
        public async Task<IActionResult> GetAllBeds(int clinicId, int departmentId)
        {
            var clinicDepartmentBeds = await _clinicDepartmentBedLogic.GetBedsForClinicDepartment(clinicId, departmentId);
            return Ok(clinicDepartmentBeds);
        }

        // Update Operation - Update the clinicDepartmentBed with the specified ID

        [HttpPatch("edit/{id}"), Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] NewClinicDepartmentBedDTO updatedClinicDepartmentBed)
        {

            if (updatedClinicDepartmentBed == null)
            {
                return BadRequest($"Error while updating an clinicDepartmentBed!");
            }

            await _clinicDepartmentBedLogic.UpdateClinicDepartmentBed(id, updatedClinicDepartmentBed);

            return Ok($"Successfully updated the clinicDepartmentBed with ID = {id}");
        }

        // Delete Operation - Delete the clinicDepartmentBed with the specified ID

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var clinicDepartmentBed = await _clinicDepartmentBedLogic.GetClinicDepartmentBedRow(id);
            if (clinicDepartmentBed == null)
            {
                return NotFound($"Could not find an clinicDepartmentBed with ID = {id}");
            }
            else
            {

                await _clinicDepartmentBedLogic.DeleteClinicDepartmentBed(id);

                return Ok($"Successfully deleted the clinicDepartmentBed with ID = {id}");
            }
        }
    }
}
