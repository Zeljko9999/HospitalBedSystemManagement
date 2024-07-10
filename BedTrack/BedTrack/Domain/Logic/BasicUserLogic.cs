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
    public class BasicUserLogic : IBasicUserLogic
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserLogic _userLogic;
        private readonly ValidationConfiguration _validationConfiguration;

        public BasicUserLogic(IUserRepository userRepository, IOptions<ValidationConfiguration> configuration, IUserLogic userLogic)
        {
            _userRepository = userRepository;
            _validationConfiguration = configuration.Value;
            _userLogic = userLogic;
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

            _userLogic.ValidateNameField(user.Name);
            _userLogic.ValidateEmailField(user.Email);

            var newUser = await _userRepository.GetUser(id);
            if (newUser == null)
            {
                throw new UserErrorMessage($"User with id {id} not found.");
            }

            newUser.Name = user.Name;
            newUser.Email = user.Email;
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
            var role = await _userRepository.GetUserRole(user);
            return UserDTO.FromModel(user, role);
        }

        public async Task<UserDTO> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            var role = await _userRepository.GetUserRole(user);
            return UserDTO.FromModel(user, role);
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var users = await _userRepository.GetAllUsers();
            var userDTOs = new List<UserDTO>();

            foreach (var user in users)
            {
                var role = await _userRepository.GetUserRole(user);
                var userDTO = UserDTO.FromModel(user, role);
                userDTOs.Add(userDTO);
            }

            return userDTOs;
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