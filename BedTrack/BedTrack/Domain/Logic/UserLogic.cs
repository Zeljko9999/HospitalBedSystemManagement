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
    public class UserLogic : IUserLogic
    {
        private readonly IUserRepository _userRepository;
        private readonly ValidationConfiguration _validationConfiguration;

        public  UserLogic(IUserRepository userRepository, IOptions<ValidationConfiguration> configuration)
        {
            _userRepository = userRepository;
            _validationConfiguration = configuration.Value;
        }

        private void ValidateNameField(string? user)
        {
            if (user is null)
            {
                throw new UserErrorMessage("Name field cannot be empty.");
            }

            if (user.Length > _validationConfiguration.NameMaxCharacters)
            {
                throw new UserErrorMessage($"User name field too long. Exceeded {_validationConfiguration.NameMaxCharacters} characters");
            }
        }

        private void ValidateEmailField(string? email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new UserErrorMessage("Email field cannot be empty.");
            }

            if (!Regex.IsMatch(email, _validationConfiguration.EmailRegex))
            {
                throw new UserErrorMessage($"Email format is invalid.");
            }
        }

        private void ValidatePasswordField(string? subject)
        {
            if (string.IsNullOrEmpty(subject))
            {
                throw new UserErrorMessage("Password cannot be empty.");
            }

            if (subject.Length < _validationConfiguration.PasswordMinCharacters)
            {
                throw new UserErrorMessage($"Password is too short. Least {_validationConfiguration.PasswordMinCharacters} characters");
            }
            if (subject.Length > _validationConfiguration.PasswordMaxCharacters)
            {
                throw new UserErrorMessage($"Password is too long. Exceeded {_validationConfiguration.PasswordMaxCharacters} characters");
            }
        }

        public async Task CreateNewUser(NewUserDTO? user)
        {
            // Convert input string fields into first letter as upppercase and all other into lowercase
            if (!(string.IsNullOrEmpty(user.Name)))
            {
                var words = user.Name.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
                user.Name = string.Join(' ', words);
            }

            ValidateNameField(user.Name);
            ValidateEmailField(user.Email);
            ValidatePasswordField(user.Password);

            var newUser = user.ToModel();
            await _userRepository.AddUser(newUser);
        }

        public async Task UpdateUser(int id, NewUserDTO? userDTO)
        {
            var user = userDTO.ToModel();

            // Convert input string fields into first letter as upppercase and all other into lowercase
            if (!(string.IsNullOrEmpty(user.Name)))
            {
                var words = user.Name.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
                user.Name = string.Join(' ', words);
            }

            ValidateNameField(user.Name);
            ValidateEmailField(user.Email);
            ValidatePasswordField(user.Password);

            var newUser = await _userRepository.GetUser(id);
            if (newUser == null)
            {
                throw new UserErrorMessage($"User with id {id} not found.");
            }

            newUser.Name = user.Name;
            newUser.Role = user.Role;
            newUser.Email = user.Email;
            newUser.Password = user.Password;
            newUser.Status = user.Status;
            newUser.ClinicId = user.ClinicId;
            newUser.DepartmentId = user.DepartmentId;

            await _userRepository.UpdateUser(newUser);
        }

        public async Task DeleteUser(int id)
        {
            _userRepository.DeleteUser(id);
        }

        public async Task<UserDTO> GetUser(int id)
        {
            var user = await _userRepository.GetUser(id);
            return user == null ? null : UserDTO.FromModel(user);
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return users.Select(UserDTO.FromModel);
        }

        public async Task<IEnumerable<string>> GetUsersByClinicId(int clinicId)
        {
            return await _userRepository.GetUsersByClinicId(clinicId);
        }

        public async Task<IEnumerable<string>> GetUsersByDepartmentId(int departmentId)
        {
            return await _userRepository.GetUsersByDepartmentId(departmentId);
        }

    }
}