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
    public class ClinicLogic : IClinicLogic
    {
        private readonly IClinicRepository _clinicRepository;
        private readonly ValidationConfiguration _validationConfiguration;

        public ClinicLogic(IClinicRepository clinicRepository, IOptions<ValidationConfiguration> configuration)
        {
            _clinicRepository = clinicRepository;
            _validationConfiguration = configuration.Value;
        }

        private void ValidateNameField(string? clinic)
        {
            if (clinic is null)
            {
                throw new UserErrorMessage("Name field cannot be empty.");
            }

            if (clinic.Length > _validationConfiguration.NameMaxCharacters)
            {
                throw new UserErrorMessage($"Clinic name field too long. Exceeded {_validationConfiguration.NameMaxCharacters} characters");
            }
        }


        private void ValidateDescriptionField(string? subject)
        {
            if (subject.Length > _validationConfiguration.DescriptionMaxCharacters)
            {
                throw new UserErrorMessage($"Description is too long. Exceeded {_validationConfiguration.DescriptionMaxCharacters} characters");
            }
        }

        public async Task CreateNewClinic(NewClinicDTO? clinic)
        {
            // Convert input string fields into first letter as upppercase and all other into lowercase
            if (!(string.IsNullOrEmpty(clinic.Name)))
            {
                var words = clinic.Name.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
                clinic.Name = string.Join(' ', words);
            }

            ValidateNameField(clinic.Name);
            ValidateDescriptionField(clinic.Description);

            var newClinic = clinic.ToModel();
            await _clinicRepository.AddClinic(newClinic);
        }

        public async Task UpdateClinic(int id, NewClinicDTO? clinicDTO)
        {
            var clinic = clinicDTO.ToModel();

            // Convert input string fields into first letter as upppercase and all other into lowercase
            if (!(string.IsNullOrEmpty(clinic.Name)))
            {
                var words = clinic.Name.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
                clinic.Name = string.Join(' ', words);
            }

            ValidateNameField(clinic.Name);
            ValidateDescriptionField(clinic.Description);

            var newClinic = await _clinicRepository.GetClinic(id);
            if (newClinic == null)
            {
                throw new UserErrorMessage($"Clinic with id {id} not found.");
            }

            newClinic.Name = clinic.Name;
            newClinic.Description = clinic.Description;

            await _clinicRepository.UpdateClinic(newClinic);
        }

        public async Task DeleteClinic(int id)
        {
            _clinicRepository.DeleteClinic(id);
        }

        public async Task<ClinicDTO> GetClinic(int id)
        {
            var clinic = await _clinicRepository.GetClinic(id);
            return clinic == null ? null : ClinicDTO.FromModel(clinic);
        }

        public async Task<IEnumerable<ClinicDTO>> GetClinics()
        {
            var clinics = await _clinicRepository.GetAllClinics();
            return clinics.Select(ClinicDTO.FromModel);
        }
    }
}