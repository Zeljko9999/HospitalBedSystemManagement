using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;
using BedTrack.Configuration;
using BedTrack.DAL.Interfaces;
using BedTrack.DAL.Repositories;
using BedTrack.Domain.Exceptions;
using BedTrack.Domain.Interfaces;
using BedTrack.Domain.Models;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace BedTrack.Domain.Logic
{
    public class ClinicDepartmentBedLogic : IClinicDepartmentBedLogic
    {
        private readonly IClinicDepartmentBedRepository _clinicDepartmentBedRepository;
        private readonly ValidationConfiguration _validationConfiguration;

        public ClinicDepartmentBedLogic(IClinicDepartmentBedRepository clinicDepartmentBedRepository, IOptions<ValidationConfiguration> configuration)
        {
            _clinicDepartmentBedRepository = clinicDepartmentBedRepository;
            _validationConfiguration = configuration.Value;
        }

        // Validate IsAvilable attribute; it must be true or false
        private async Task ValidateIsAvailableField(bool availability)
        {   
            if (availability == null)
            {
                throw new UserErrorMessage("IsAvailable cannot be null");
            }

            if (availability != true && availability != false)
            {
                throw new UserErrorMessage("IsAvailable must be true or false!");
            }
        }


        //Validating foreign keys to protect of duplicating ClinicDepartments with same bed and avoid duplicating of patients
        private async Task ValidateIdFields(int clinicDepartmentId, int bedId, int? patientId)
        {
            var existingClinicDepartmentBed = await _clinicDepartmentBedRepository.GetClinicDepartmentForBed(clinicDepartmentId, bedId);
            if (existingClinicDepartmentBed != null)
            {
                throw new UserErrorMessage("ClinicDepartment with that bed already exists.");
            }

            if (patientId.HasValue)
            {
                var existingClinicDepartmentPatient = await _clinicDepartmentBedRepository.GetClinicDepartmentForPatient(patientId.Value);
                if (existingClinicDepartmentPatient != null)
                {
                    throw new UserErrorMessage("ClinicDepartment with that patient already exists.");
                }
            }
        }

        public async Task CreateNewClinicDepartmentBed(NewClinicDepartmentBedDTO? clinicDepartmentBed)
        {
            await ValidateIsAvailableField(clinicDepartmentBed.IsAvailable);
            await ValidateIdFields(clinicDepartmentBed.ClinicDepartmentId, clinicDepartmentBed.BedId, clinicDepartmentBed.PatientId);

            var newClinicDepartmentBed = clinicDepartmentBed.ToModel();
            await _clinicDepartmentBedRepository.AddClinicDepartmentBed(newClinicDepartmentBed);
        }

        public async Task UpdateClinicDepartmentBed(int id, NewClinicDepartmentBedDTO? clinicDepartmentBedDTO)
        {
            var clinicDepartmentBed = clinicDepartmentBedDTO.ToModel();


            await ValidateIsAvailableField(clinicDepartmentBed.IsAvailable);
            await ValidateIdFields(clinicDepartmentBed.ClinicDepartmentId, clinicDepartmentBed.BedId, clinicDepartmentBed.PatientId);


            var newClinicDepartmentBed = await _clinicDepartmentBedRepository.GetClinicDepartmentBed(id);
            if (newClinicDepartmentBed == null)
            {
                throw new UserErrorMessage($"ClinicDepartmentBed with ID's {id} not found.");
            }

            newClinicDepartmentBed.IsAvailable = clinicDepartmentBed.IsAvailable;
            newClinicDepartmentBed.Status = clinicDepartmentBed.Status;
            newClinicDepartmentBed.ClinicDepartmentId = clinicDepartmentBed.ClinicDepartmentId;
            newClinicDepartmentBed.BedId = clinicDepartmentBed.BedId;
            newClinicDepartmentBed.PatientId = clinicDepartmentBed.PatientId;


            await _clinicDepartmentBedRepository.UpdateClinicDepartmentBed(newClinicDepartmentBed);
        }

        public async Task DeleteClinicDepartmentBed(int id)
        {
            _clinicDepartmentBedRepository.DeleteClinicDepartmentBed(id);
        }

        public async Task<ClinicDepartmentBedDTO> GetClinicDepartmentBedRow(int id)
        {
            var clinicDepartmentBed = await _clinicDepartmentBedRepository.GetClinicDepartmentBed(id);
            return clinicDepartmentBed == null ? null : ClinicDepartmentBedDTO.FromModel(clinicDepartmentBed);
        }
    }
}
