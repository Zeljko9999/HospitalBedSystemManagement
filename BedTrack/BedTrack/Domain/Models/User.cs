using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BedTrack.Domain.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }

        public string? Status { get; set; }

        public int ClinicId { get; set; }
        public Clinic Clicnic { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<UserEvent>? AlarmEvents { get; set; }
        public ICollection<UserPatient>? Patients { get; set; }
    }
}
