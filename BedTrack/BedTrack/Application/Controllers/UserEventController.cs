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
    public class UserEventController : ControllerBase
    {
        private readonly IUserEventLogic _userEventLogic;

        public UserEventController(IUserEventLogic userEventLogic)
        {
            this._userEventLogic = userEventLogic;
        }

        // Create an UserEvent object

        [HttpPost, Authorize]
        public async Task<IActionResult> Post([FromBody] NewUserEventDTO userEvent)
        {

            if (userEvent != null)
            {
                await _userEventLogic.CreateNewUserEvent(userEvent);
                return Ok("UserEvent successfully created!");
            }
            else
            {
                return BadRequest("Error while creating an UserEvent!");
            }
        }

        // Read Operation 1 - Get all events of user with the specified ID

        [HttpGet("user/{userId}/events"), Authorize]
        public async Task<IActionResult> GetAll(int userId)
        {
            var userEvents = await _userEventLogic.GetUserEvents(userId);
            return Ok(userEvents);
        }

        // Read Operation 2 - Get the closest event of user with the specified ID

        [HttpGet("{userId}"), Authorize]
        public async Task<IActionResult> Get(int userId)
        {
            var userEvent = await _userEventLogic.GetUserEvent(userId);

            if (userEvent is null)
            {
                return NotFound($"Could not find an event of user with ID {userId}");
            }
            else
            {
                return Ok(userEvent);
            }
        }

        // Update Operation - Update the event with the specified ID

        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] NewUserEventDTO updatedUserEvent)
        {

            if (updatedUserEvent == null)
            {
                return BadRequest($"Error while updating an UserEvent!");
            }

            await _userEventLogic.UpdateUserEvent(id, updatedUserEvent);

            return Ok($"Successfully updated the UserEvent with ID = {id}");
        }

        // Delete Operation - Delete the UserEvent with the specified ID

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userEvent = await _userEventLogic.GetUserEventRow(id);
            if (userEvent == null)
            {
                return NotFound($"Could not find an event with ID = {id}");
            }
            else
            {

                await _userEventLogic.DeleteUserEvent(id);

                return Ok($"Successfully deleted the event with ID = {id}");
            }
        }
    }
}
