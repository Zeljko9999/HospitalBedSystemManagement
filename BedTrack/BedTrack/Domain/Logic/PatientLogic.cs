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
    public class PatientLogic : IPatientLogic
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ValidationConfiguration _validationConfiguration;

        public PatientLogic(IPatientRepository patientRepository, IOptions<ValidationConfiguration> configuration)
        {
            _patientRepository = patientRepository;
            _validationConfiguration = configuration.Value;
        }

        private void ValidateNameField(string? patient)
        {
            if (patient is null)
            {
                throw new UserErrorMessage("Name field cannot be empty.");
            }

            if (patient.Length > _validationConfiguration.NameMaxCharacters)
            {
                throw new UserErrorMessage($"Patient name field is too long. Exceeded {_validationConfiguration.NameMaxCharacters} characters");
            }
        }

        private void ValidateAgeField(int? age)
        {

            if (age < 0)
            {
                throw new UserErrorMessage($"Age can not be negative.");
            }
        }

        public async Task CreateNewPatient(NewPatientDTO? patient)
        {
            // Convert input string fields into first letter as upppercase and all other into lowercase
            if (!(string.IsNullOrEmpty(patient.Name)))
            {
                var words = patient.Name.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
                patient.Name = string.Join(' ', words);
            }

            ValidateNameField(patient.Name);
            ValidateAgeField(patient.Age);

            var newPatient = patient.ToModel();
            await _patientRepository.AddPatient(newPatient);
        }

        public async Task UpdatePatient(int id, NewPatientDTO? patientDTO)
        {
            var patient = patientDTO.ToModel();

            // Convert input string fields into first letter as upppercase and all other into lowercase
            if (!(string.IsNullOrEmpty(patient.Name)))
            {
                var words = patient.Name.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
                patient.Name = string.Join(' ', words);
            }

            ValidateNameField(patient.Name);
            ValidateAgeField(patient.Age);

            var newPatient = await _patientRepository.GetPatient(id);
            if (newPatient == null)
            {
                throw new UserErrorMessage($"Patient with id {id} not found.");
            }

            newPatient.Name = patient.Name;
            newPatient.Age = patient.Age;
            newPatient.Nationality = patient.Nationality;
            newPatient.Insurance = patient.Insurance;
            newPatient.HealthRecord = patient.HealthRecord;
            newPatient.HealthHistory = patient.HealthHistory;

            await _patientRepository.UpdatePatient(newPatient);
        }

        public async Task DeletePatient(int id)
        {
            _patientRepository.DeletePatient(id);
        }

        public async Task<IEnumerable<PatientDTO>> GetAllPatients()
        {
            var patients = await _patientRepository.GetAllPatients();
            return patients.Select(PatientDTO.FromModel);
        }

        public async Task<PatientDTO> GetPatient(int id)
        {
            var patient = await _patientRepository.GetPatient(id);
            return patient == null ? null : PatientDTO.FromModel(patient);
        }

        public async Task<IEnumerable<PatientDTO>> GetPatientsWithoutBed()
        {
            var patients = await _patientRepository.GetPatientsWithoutBeds();
            return patients.Select(PatientDTO.FromModel);
        }
    }
}
