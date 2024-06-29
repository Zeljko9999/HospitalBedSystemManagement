using BedTrack.Domain.Models;

namespace BedTrack.Application.DTO
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Nationality { get; set; }
        public int? Age { get; set; }
        public string Insurance { get; set; }
        public string? HealthRecord { get; set; }
        public string? HealthHistory { get; set; }
        public int ClinicDepartmentBedId { get; set; }   

    public static PatientDTO FromModel(Patient model)
        {
            return new PatientDTO
            {
                Id = model.Id,
                Name = model.Name,
                Nationality = model.Nationality,
                Age = model.Age,
                Insurance = model.Insurance,
                HealthRecord = model.HealthRecord,
                HealthHistory = model.HealthHistory,
                ClinicDepartmentBedId = model.ClinicDepartmentBedId,
            };
        }
    }
}
