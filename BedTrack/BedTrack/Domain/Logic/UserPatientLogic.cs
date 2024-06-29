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
    public class UserPatientLogic : IUserPatientLogic
    {
        private readonly IUserPatientRepository _userPatientRepository;
        private readonly ValidationConfiguration _validationConfiguration;

        public UserPatientLogic(IUserPatientRepository userPatientRepository, IOptions<ValidationConfiguration> configuration)
        {
            _userPatientRepository = userPatientRepository;
            _validationConfiguration = configuration.Value;
        }

        //Validating foreign keys to protect of duplicating patients for same user
        private async Task ValidateIdFields(int userId, int patientId)
        {
            var existingUserPatient = await _userPatientRepository.GetPatientForUser(userId, patientId);
            if (existingUserPatient != null)
            {
                throw new UserErrorMessage("User with that patient already exists.");
            }

        }

        public async Task CreateNewUserPatient(NewUserPatientDTO? userPatient)
        {
            await ValidateIdFields(userPatient.UserId, userPatient.PatientId);


            var newUserPatient = userPatient.ToModel();
            await _userPatientRepository.AddUserPatient(newUserPatient);
        }

        public async Task UpdateUserPatient(int id, NewUserPatientDTO? userPatientDTO)
        {
            var userPatient = userPatientDTO.ToModel();

            ValidateIdFields(userPatient.UserId, userPatient.PatientId);

            var newUserPatient = await _userPatientRepository.GetUserPatient(id);
            if (newUserPatient == null)
            {
                throw new UserErrorMessage($"UserPatient with ID's {id} not found.");
            }

            newUserPatient.UserId = userPatient.UserId;
            newUserPatient.PatientId = userPatient.PatientId;

            await _userPatientRepository.UpdateUserPatient(newUserPatient);
            
        }
        public async Task DeleteUserPatient(int id)
        {
            _userPatientRepository.DeleteUserPatient(id);
        }

     
        public async Task<UserPatientDTO> GetUserPatientRow(int id)
        {
            var userPatient = await _userPatientRepository.GetUserPatient(id);
            return userPatient == null ? null : UserPatientDTO.FromModel(userPatient);
        }

        public async Task<IEnumerable<UserPatientDTO>> GetPatientsForUser(int userId)
        {
            var userPatients = await _userPatientRepository.GetPatientsForUser(userId);
            return userPatients.Select(UserPatientDTO.FromModel);
        }

        public async Task<IEnumerable<UserPatientDTO>> GetUsersForPatient(int patientId)
        {
            var patientUsers = await _userPatientRepository.GetPatientsForUser(patientId);
            return patientUsers.Select(UserPatientDTO.FromModel);
        }

    }
}
