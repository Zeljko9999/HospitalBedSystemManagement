using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;
using BedTrack.Configuration;
using BedTrack.DAL.Interfaces;
using BedTrack.Domain.Exceptions;
using BedTrack.Domain.Interfaces;
using BedTrack.Domain.Models;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace BedTrack.Domain.Logic
{
    public class ClinicDepartmentLogic : IClinicDepartmentLogic
    {
        private readonly IClinicDepartmentRepository _clinicDepartmentRepository;
        private readonly ValidationConfiguration _validationConfiguration;

        public ClinicDepartmentLogic(IClinicDepartmentRepository clinicDepartmentRepository, IOptions<ValidationConfiguration> configuration)
        {
            _clinicDepartmentRepository = clinicDepartmentRepository;
            _validationConfiguration = configuration.Value;
        }

        private void ValidateQuantityField(int? quantity)
        {
            if (quantity is null)
            {
                throw new UserErrorMessage("Quantity field cannot be empty.");
            }

            if (quantity < 0)
            {
                throw new UserErrorMessage($"Quantity can not be negative.");
            }
        }

        //Validating foreign keys to protect of duplicating departments on same clinic
        private async Task ValidateIdFields(int clinicId, int departmentId)
        {
            var existingClinicDepartment = await _clinicDepartmentRepository.GetDepartmentForClinic(clinicId, departmentId);
            if (existingClinicDepartment != null)
            {
                throw new UserErrorMessage("Clinic with that department already exists.");
            }
 
        }

        private void ValidateDescriptionField(string? subject)
        {
             if (subject.Length > _validationConfiguration.DescriptionMaxCharacters)
             {       
                 throw new UserErrorMessage($"Description is too long. Exceeded {_validationConfiguration.DescriptionMaxCharacters} characters");
             }   
        }


        public async Task CreateNewClinicDepartment(NewClinicDepartmentDTO? clinicDepartment)
        {
            await ValidateIdFields(clinicDepartment.ClinicId, clinicDepartment.DepartmentId);
            ValidateQuantityField(clinicDepartment.BedQuantity);
            ValidateDescriptionField(clinicDepartment.Description);

            var newClinicDepartment = clinicDepartment.ToModel();
            await _clinicDepartmentRepository.AddClinicDepartment(newClinicDepartment);
        }

        public async Task UpdateClinicDepartment(int id, NewClinicDepartmentDTO? clinicDepartmentDTO)
        {
            var clinicDepartment = clinicDepartmentDTO.ToModel();

            ValidateIdFields(clinicDepartment.ClinicId, clinicDepartment.DepartmentId);
            ValidateQuantityField(clinicDepartment.BedQuantity);
            ValidateDescriptionField(clinicDepartment.Description);

            var newClinicDepartment = await _clinicDepartmentRepository.GetClinicDepartment(id);
            if (newClinicDepartment == null)
            {
                throw new UserErrorMessage($"ClinicDepartment with ID's {id} not found.");
            }

            newClinicDepartment.BedQuantity = clinicDepartment.BedQuantity;
            newClinicDepartment.Description = clinicDepartment.Description;
            newClinicDepartment.ClinicId = clinicDepartment.ClinicId;
            newClinicDepartment.DepartmentId = clinicDepartment.DepartmentId;

            await _clinicDepartmentRepository.UpdateClinicDepartment(newClinicDepartment);
        }

        public async Task DeleteClinicDepartment(int id)
        {
            _clinicDepartmentRepository.DeleteClinicDepartment(id);
        }

        public async Task<ClinicDepartmentDTO> GetClinicDepartment(int clinicId, int departmentId)
        {
            var clinicDepartment = await _clinicDepartmentRepository.GetDepartmentForClinic(clinicId, departmentId);
            return clinicDepartment == null ? null : ClinicDepartmentDTO.FromModel(clinicDepartment);
        }

        public async Task<IEnumerable<ClinicDepartmentDTO>> GetClinicDepartments(int clinicId)
        {
            var clinicDepartments = await _clinicDepartmentRepository.GetAllDepartmentsOfClinic(clinicId);
            return clinicDepartments.Select(ClinicDepartmentDTO.FromModel);
        }

        public async Task<ClinicDepartmentDTO> GetClinicDepartmentRow(int id)
        {
            var clinicDepartment = await _clinicDepartmentRepository.GetClinicDepartment(id);
            return clinicDepartment == null ? null : ClinicDepartmentDTO.FromModel(clinicDepartment);
        }
    }
}
