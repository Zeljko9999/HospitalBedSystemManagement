using BedTrack.Domain.Models;

namespace BedTrack.Application.NewDTO
{
    public class NewPatientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Nationality { get; set; }
        public int? Age { get; set; }
        public string Insurance { get; set; }
        public string? HealthRecord { get; set; }
        public string? HealthHistory { get; set; }
        public int ClinicDepartmentBedId { get; set; }

        public Patient ToModel()
        {
            return new Patient
            {
                Name = Name,
                Nationality = Nationality,
                Age = Age,
                Insurance = Insurance,
                HealthRecord = HealthRecord,
                HealthHistory = HealthHistory,
                ClinicDepartmentBedId = ClinicDepartmentBedId,
                Users = null,
                ClinicDepartmentBed = null
            };
        }
    }
}
