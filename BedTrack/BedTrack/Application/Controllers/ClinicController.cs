﻿using BedTrack.Application.NewDTO;
using BedTrack.Domain.Interfaces;
using FactoryApplication.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BedTrack.Application.Controllers
{
    [ErrorFilter]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClinicController : ControllerBase
    {
        private readonly IClinicLogic _clinicLogic;

        public ClinicController(IClinicLogic clinicLogic)
        {
            this._clinicLogic = clinicLogic;
        }

        // Create an clinic object

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewClinicDTO clinic)
        {

            if (clinic != null)
            {
                await _clinicLogic.CreateNewClinic(clinic);
                return Ok("Clinic successfully created!");
            }
            else
            {
                return BadRequest("Error while creating an clinic!");
            }
        }

        // Read Operation 1 - Get all clinics

        [HttpGet, Authorize(Roles = "Admin, Boss")]
        public async Task<IActionResult> GetAll()
        {
            var clinics = await _clinicLogic.GetClinics();
            return Ok(clinics);
        }

        // Read Operation 2 - Get the clinic with the specified ID

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var clinic = await _clinicLogic.GetClinic(id);

            if (clinic is null)
            {
                return NotFound($"Could not find an clinic with ID = {id}");
            }
            else
            {
                return Ok(clinic);
            }
        }

        // Update Operation - Update the clinic with the specified ID

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NewClinicDTO updatedClinic)
        {

            if (updatedClinic == null)
            {
                return BadRequest($"Error while updating an clinic!");
            }

            await _clinicLogic.UpdateClinic(id, updatedClinic);

            return Ok($"Successfully updated the clinic with ID = {id}");
        }

        // Delete Operation - Delete the clinic with the specified ID

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var clinic = await _clinicLogic.GetClinic(id);
            if (clinic == null)
            {
                return NotFound($"Could not find an clinic with ID = {id}");
            }
            else
            {

                await _clinicLogic.DeleteClinic(id);

                return Ok($"Successfully deleted the clinic with ID = {id}");
            }
        }
    }
}
