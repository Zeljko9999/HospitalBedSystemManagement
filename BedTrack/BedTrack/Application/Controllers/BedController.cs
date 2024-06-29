using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using BedTrack.Domain.Models;
using BedTrack.Domain.Logic;
using BedTrack.Domain.Interfaces;
using BedTrack.Application.NewDTO;
using FactoryApplication.Filters;

namespace BedTrack.Application.Controllers
{
    [ErrorFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class BedController : ControllerBase
    {
        private readonly IBedLogic _bedLogic;

        public BedController(IBedLogic bedLogic)
        {
            this._bedLogic = bedLogic;
        }

        // Create an bed object

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewBedDTO bed)
        {

            if (bed != null)
            {
                await _bedLogic.CreateNewBed(bed);
                return Ok("Bed successfully created!");
            }
            else
            {
                return BadRequest("Error while creating an bed!");
            }
        }

        // Read Operation 1 - Get all beds

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var beds = await _bedLogic.GetBeds();
            return Ok(beds);
        }

        // Read Operation 2 - Get the bed with the specified ID

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var bed = await _bedLogic.GetBed(id);

            if (bed is null)
            {
                return NotFound($"Could not find an bed with ID = {id}");
            }
            else
            {
                return Ok(bed);
            }
        }

        // Update Operation - Update the bed with the specified ID

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NewBedDTO updatedBed)
        {

            if (updatedBed == null)
            {
                return BadRequest($"Error while updating an bed!");
            }

            await _bedLogic.UpdateBed(id, updatedBed);

            return Ok($"Successfully updated the bed with ID = {id}");
        }

        // Delete Operation - Delete the bed with the specified ID

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bed = await _bedLogic.GetBed(id);
            if (bed == null)
            {
                return NotFound($"Could not find an bed with ID = {id}");
            }
            else
            {

                await _bedLogic.DeleteBed(id);

                return Ok($"Successfully deleted the bed with ID = {id}");
            }
        }
    }
}
