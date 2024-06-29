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
    public class UserEventLogic : IUserEventLogic
    {
        private readonly IUserEventRepository _userEventRepository;
        private readonly ValidationConfiguration _validationConfiguration;

        public UserEventLogic(IUserEventRepository userEventRepository, IOptions<ValidationConfiguration> configuration)
        {
            _userEventRepository = userEventRepository;
            _validationConfiguration = configuration.Value;
        }

        public async Task CreateNewUserEvent(NewUserEventDTO? userEvent)
        { 
            var newUserEvent = userEvent.ToModel();
            await _userEventRepository.AddUserEvent(newUserEvent);
        }

        public async Task UpdateUserEvent(int id, NewUserEventDTO? userEventDTO)
        {
            var userEvent = userEventDTO.ToModel();

            var newUserEvent = await _userEventRepository.GetUserEvent(id);
            if (newUserEvent == null)
            {
                throw new UserErrorMessage($"UserEvent with ID's {id} not found.");
            }

            newUserEvent.UserId = userEvent.UserId;
            newUserEvent.EventId = userEvent.EventId;

            await _userEventRepository.UpdateUserEvent(newUserEvent);
        }

        public async Task DeleteUserEvent(int id)
        {
            _userEventRepository.DeleteUserEvent(id);
        }

        public async Task<EventDTO?> GetUserEvent(int userId)
        {
            var userEvent = await _userEventRepository.GetEventForUser(userId);
            return userEvent == null ? null : EventDTO.FromModel(userEvent);
        }

        public async Task<IEnumerable<EventDTO>> GetUserEvents(int userId)
        {
            var userEvents = await _userEventRepository.GetAllEventsOfUser(userId);
            return userEvents.Select(EventDTO.FromModel);
        }

        public async Task<UserEventDTO?> GetUserEventRow(int id)
        {
            var userEvent = await _userEventRepository.GetUserEvent(id);
            return userEvent == null ? null : UserEventDTO.FromModel(userEvent);
        }
    }
}
