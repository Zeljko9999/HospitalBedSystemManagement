using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using BedTrack.Domain.Models;
using BedTrack.DAL.Repositories;
using BedTrack.DAL.Interfaces;
using BedTrack.Domain.Logic;
using BedTrack.Domain.Interfaces;
using BedTrack.Application.NewDTO;
using FactoryApplication.Filters;
using Microsoft.AspNetCore.Authorization;

namespace BedTrack.Application.Controllers
{
    [ErrorFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEvenetLogic _eventLogic;

        public EventController(IEvenetLogic eventLogic)
        {
            this._eventLogic = eventLogic;
        }

        // Create an event object

        [HttpPost, Authorize]
        public async Task<IActionResult> Post([FromBody] NewEventDTO eventt)
        {

            if (eventt != null)
            {
                await _eventLogic.CreateNewEvent(eventt);
                return Ok("Event successfully created!");
            }
            else
            {
                return BadRequest("Error while creating an event!");
            }
        }


        // Read Operation 2 - Get the event with the specified ID

        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var eventt = await _eventLogic.GetEvent(id);

            if (eventt is null)
            {
                return NotFound($"Could not find an event with ID = {id}");
            }
            else
            {
                return Ok(eventt);
            }
        }

        // Update Operation - Update the event with the specified ID

        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] NewEventDTO updatedEvent)
        {

            if (updatedEvent == null)
            {
                return BadRequest($"Error while updating an event!");
            }

            await _eventLogic.UpdateEvent(id, updatedEvent);

            return Ok($"Successfully updated the event with ID = {id}");
        }

        // Delete Operation - Delete the event with the specified ID

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var eventt = await _eventLogic.GetEvent(id);
            if (eventt == null)
            {
                return NotFound($"Could not find an event with ID = {id}");
            }
            else
            {

                await _eventLogic.DeleteEvent(id);

                return Ok($"Successfully deleted the event with ID = {id}");
            }
        }
    }
}
