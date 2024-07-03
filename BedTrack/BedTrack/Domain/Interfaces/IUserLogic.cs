using BedTrack.Domain.Models;
using BedTrack.Application.DTO;
using BedTrack.Application.NewDTO;

namespace BedTrack.Domain.Interfaces
{
    public interface IUserLogic
    {
        Task ValidateNameField(string? user);
        Task ValidateEmailField(string? email);
    }
}
