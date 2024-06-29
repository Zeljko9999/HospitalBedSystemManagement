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
    public class EventLogic : IEvenetLogic
    {
        private readonly IEventRepository _eventRepository;
        private readonly ValidationConfiguration _validationConfiguration;

        public EventLogic(IEventRepository eventRepository, IOptions<ValidationConfiguration> configuration)
        {
            _eventRepository = eventRepository;
            _validationConfiguration = configuration.Value;
        }

        private void ValidateNameField(string? eventt)
        {
            if (eventt is null)
            {
                throw new UserErrorMessage("Name field cannot be empty.");
            }

            if (eventt.Length > _validationConfiguration.NameMaxCharacters)
            {
                throw new UserErrorMessage($"Event name field too long. Exceeded {_validationConfiguration.NameMaxCharacters} characters");
            }
        }

        private void ValidateDescriptionField(string? subject)
        {
            if (subject.Length > _validationConfiguration.DescriptionMaxCharacters)
            {
                throw new UserErrorMessage($"Description is too long. Exceeded {_validationConfiguration.DescriptionMaxCharacters} characters");
            }
        }

        private void ValidateTimeField(DateTime? time)
        {
            if (time is null)
            {
                throw new UserErrorMessage("Time field cannot be empty.");
            }

            DateTime minTime = DateTime.Now;

            if (time < minTime)
            {
                throw new UserErrorMessage("Wrong data. Time should be newer than now.");
            }
        }

        public async Task CreateNewEvent(NewEventDTO? eventt)
        {
            // Convert input string fields into first letter as upppercase and all other into lowercase
            if (!(string.IsNullOrEmpty(eventt.Name)))
            {
                var words = eventt.Name.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
                eventt.Name = string.Join(' ', words);
            }

            ValidateNameField(eventt.Name);
            ValidateDescriptionField(eventt.Description);
            ValidateTimeField(eventt.Alarm);

            var newEvent = eventt.ToModel();
            await _eventRepository.AddEvent(newEvent);
        }

        public async Task UpdateEvent(int id, NewEventDTO? eventDTO)
        {
            var eventt = eventDTO.ToModel();

            // Convert input string fields into first letter as upppercase and all other into lowercase
            if (!(string.IsNullOrEmpty(eventt.Name)))
            {
                var words = eventt.Name.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
                eventt.Name = string.Join(' ', words);
            }

            ValidateNameField(eventt.Name);
            ValidateDescriptionField(eventt.Description);
            ValidateTimeField(eventt.Alarm);

            var newEvent = await _eventRepository.GetEvent(id);
            if (newEvent == null)
            {
                throw new UserErrorMessage($"Event with id {id} not found.");
            }

            newEvent.Name = eventt.Name;
            newEvent.Description = eventt.Description;
            newEvent.Alarm = eventt.Alarm;

            await _eventRepository.UpdateEvent(newEvent);
        }

        public async Task DeleteEvent(int id)
        {
            _eventRepository.DeleteEvent(id);
        }

        public async Task<EventDTO> GetEvent(int id)
        {
            var eventt = await _eventRepository.GetEvent(id);
            return eventt == null ? null : EventDTO.FromModel(eventt);
        }
    }
}
