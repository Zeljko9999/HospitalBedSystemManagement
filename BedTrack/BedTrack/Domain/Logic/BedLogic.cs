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
    public class BedLogic : IBedLogic
    {
        private readonly IBedRepository _bedRepository;
        private readonly ValidationConfiguration _validationConfiguration;

        public BedLogic(IBedRepository bedRepository, IOptions<ValidationConfiguration> configuration)
        {
            _bedRepository = bedRepository;
            _validationConfiguration = configuration.Value;
        }

        private void ValidateNameField(string? bed)
        {
            if (bed is null)
            {
                throw new UserErrorMessage("Name field cannot be empty.");
            }

            if (bed.Length > _validationConfiguration.NameMaxCharacters)
            {
                throw new UserErrorMessage($"Bed name field too long. Exceeded {_validationConfiguration.NameMaxCharacters} characters");
            }
        }


        public async Task CreateNewBed(NewBedDTO? bed)
        {
            // Convert input string fields into first letter as upppercase and all other into lowercase
            if (!(string.IsNullOrEmpty(bed.Name)))
            {
                var words = bed.Name.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
                bed.Name = string.Join(' ', words);
            }

            ValidateNameField(bed.Name);

            var newBed = bed.ToModel();
            await _bedRepository.AddBed(newBed);
        }

        public async Task UpdateBed(int id, NewBedDTO? bedDTO)
        {
            var bed = bedDTO.ToModel();

            // Convert input string fields into first letter as upppercase and all other into lowercase
            if (!(string.IsNullOrEmpty(bed.Name)))
            {
                var words = bed.Name.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
                bed.Name = string.Join(' ', words);
            }

            ValidateNameField(bed.Name);

            var newBed = await _bedRepository.GetBed(id);
            if (newBed == null)
            {
                throw new UserErrorMessage($"Bed with id {id} not found.");
            }

            newBed.Name = bed.Name;

            await _bedRepository.UpdateBed(newBed);
        }

        public async Task DeleteBed(int id)
        {
            _bedRepository.DeleteBed(id);
        }

        public async Task<BedDTO> GetBed(int id)
        {
            var bed = await _bedRepository.GetBed(id);
            return bed == null ? null : BedDTO.FromModel(bed);
        }

        public async Task<IEnumerable<BedDTO>> GetBeds()
        {
            var beds = await _bedRepository.GetAllBeds();
            return beds.Select(BedDTO.FromModel);
        }
    }
}
