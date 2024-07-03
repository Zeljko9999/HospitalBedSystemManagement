using System.Text.RegularExpressions;
using BedTrack.Domain.Exceptions;
using BedTrack.Domain.Models;
using BedTrack.Domain.Interfaces;
using BedTrack.Configuration;
using Microsoft.Extensions.Options;
using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;

namespace BedTrack.Domain.Logic
{
    public class UserLogic : IUserLogic
    {
        private readonly ValidationConfiguration _validationConfiguration;

        public  UserLogic(IOptions<ValidationConfiguration> configuration)
        {
            _validationConfiguration = configuration.Value;
        }

        public async Task ValidateNameField(string? user)
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

        public async Task ValidateEmailField(string? email)
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
    }
}