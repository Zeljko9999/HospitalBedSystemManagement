using System.Text.RegularExpressions;
using BedTrack.Domain.Exceptions;
using BedTrack.Domain.Models;
using BedTrack.DAL.Repositories;
using BedTrack.Domain.Interfaces;
using BedTrack.DAL.Interfaces;
using BedTrack.Configuration;
using Microsoft.Extensions.Options;
using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;


namespace BedTrack.Domain.Logic
{
    public class DepartmentLogic : IDepartmentLogic
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ValidationConfiguration _validationConfiguration;

        public DepartmentLogic(IDepartmentRepository departmentRepository, IOptions<ValidationConfiguration> configuration)
        {
            _departmentRepository = departmentRepository;
            _validationConfiguration = configuration.Value;
        }

        private void ValidateNameField(string? department)
        {
            if (department is null)
            {
                throw new UserErrorMessage("Name field cannot be empty.");
            }

            if (department.Length > _validationConfiguration.NameMaxCharacters)
            {
                throw new UserErrorMessage($"Department name field too long. Exceeded {_validationConfiguration.NameMaxCharacters} characters");
            }
        }


        public async Task CreateNewDepartment(NewDepartmentDTO? department)
        {
            // Convert input string fields into first letter as upppercase and all other into lowercase
            if (!(string.IsNullOrEmpty(department.Name)))
            {
                var words = department.Name.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
                department.Name = string.Join(' ', words);
            }

            ValidateNameField(department.Name);

            var newDepartment = department.ToModel();
            await _departmentRepository.AddDepartment(newDepartment);
        }

        public async Task UpdateDepartment(int id, NewDepartmentDTO? departmentDTO)
        {
            var department = departmentDTO.ToModel();

            // Convert input string fields into first letter as upppercase and all other into lowercase
            if (!(string.IsNullOrEmpty(department.Name)))
            {
                var words = department.Name.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
                department.Name = string.Join(' ', words);
            }

            ValidateNameField(department.Name);

            var newDepartment = await _departmentRepository.GetDepartment(id);
            if (newDepartment == null)
            {
                throw new UserErrorMessage($"Department with id {id} not found.");
            }

            newDepartment.Name = department.Name;

            await _departmentRepository.UpdateDepartment(newDepartment);
        }

        public async Task DeleteDepartment(int id)
        {
            _departmentRepository.DeleteDepartment(id);
        }

        public async Task<DepartmentDTO> GetDepartment(int id)
        {
            var department = await _departmentRepository.GetDepartment(id);
            return department == null ? null : DepartmentDTO.FromModel(department);
        }

        public async Task<IEnumerable<DepartmentDTO>> GetDepartments()
        {
            var departments = await _departmentRepository.GetAllDepartments();
            return departments.Select(DepartmentDTO.FromModel);
        }
    }
}