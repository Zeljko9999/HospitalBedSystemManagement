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
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentLogic _departmentLogic;

        public DepartmentController(IDepartmentLogic departmentLogic)
        {
            this._departmentLogic = departmentLogic;
        }

        // Create an department object

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewDepartmentDTO department)
        {

            if (department != null)
            {
                await _departmentLogic.CreateNewDepartment(department);
                return Ok("Department successfully created!");
            }
            else
            {
                return BadRequest("Error while creating an department!");
            }
        }

        // Read Operation 1 - Get all departments

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentLogic.GetDepartments();
            return Ok(departments);
        }

        // Read Operation 2 - Get the department with the specified ID

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var department = await _departmentLogic.GetDepartment(id);

            if (department is null)
            {
                return NotFound($"Could not find an department with ID = {id}");
            }
            else
            {
                return Ok(department);
            }
        }

        // Update Operation - Update the department with the specified ID

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NewDepartmentDTO updatedDepartment)
        {

            if (updatedDepartment == null)
            {
                return BadRequest($"Error while updating an department!");
            }

            await _departmentLogic.UpdateDepartment(id, updatedDepartment);

            return Ok($"Successfully updated the department with ID = {id}");
        }

        // Delete Operation - Delete the department with the specified ID

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentLogic.GetDepartment(id);
            if (department == null)
            {
                return NotFound($"Could not find an department with ID = {id}");
            }
            else
            {

                await _departmentLogic.DeleteDepartment(id);

                return Ok($"Successfully deleted the department with ID = {id}");
            }
        }
    }
}
