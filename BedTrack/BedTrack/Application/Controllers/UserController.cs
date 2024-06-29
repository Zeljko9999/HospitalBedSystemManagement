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

namespace BedTrack.Application.Controllers
{
    [ErrorFilter]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserLogic _userLogic;

        public UserController(IUserLogic userLogic)
        {
            this._userLogic = userLogic;
        }

        // Create an user object

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewUserDTO user)
        {

            if (user != null)
            {
                await _userLogic.CreateNewUser(user);
                return Ok("User successfully created!");
            }
            else
            {
                return BadRequest("Error while creating an user!");
            }
        }

        // Read Operation 1 - Get all users

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userLogic.GetUsers();
            return Ok(users);
        }

        // Read Operation 2 - Get the user with the specified ID

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userLogic.GetUser(id);

            if (user is null)
            {
                return NotFound($"Could not find an user with ID = {id}");
            }
            else
            {
                return Ok(user);
            }
        }

        // Update Operation - Update the user with the specified ID

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NewUserDTO updatedUser)
        {

            if (updatedUser == null)
            {
                return BadRequest($"Error while updating an user!");
            }

            await _userLogic.UpdateUser(id, updatedUser);

            return Ok($"Successfully updated the user with ID = {id}");
        }

        // Delete Operation - Delete the user with the specified ID

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userLogic.GetUser(id);
            if (user == null)
                {
                    return NotFound($"Could not find an user with ID = {id}");
                }
            else
                { 

                await _userLogic.DeleteUser(id);

                return Ok($"Successfully deleted the user with ID = {id}");
                }
        }

        [HttpGet("clinic/{clinicId}/names")]
        public async Task<ActionResult<IEnumerable<string>>> GetUserNamesByClinicId(int clinicId)
        {
            var userNames = await _userLogic.GetUsersByClinicId(clinicId);
            return Ok(userNames);
        }


        [HttpGet("department/{departmentId}/names")]
        public async Task<ActionResult<IEnumerable<string>>> GetUserNamesByDepartmentId(int departmentId)
        {
            var userNames = await _userLogic.GetUsersByClinicId(departmentId);
            return Ok(userNames);
        }
    }
}
