using BedTrack.Domain.Models;

namespace BedTrack.Application.NewDTO
{
    public class NewClinicDepartmentDTO
    {
        public int BedQuantity { get; set; }
        public string Description { get; set; }
        public int ClinicId { get; set; }
        public int DepartmentId { get; set; }


        public ClinicDepartment ToModel()
        {
            return new ClinicDepartment
            {
                BedQuantity = BedQuantity,
                Description = Description,
                ClinicId = ClinicId,
                DepartmentId = DepartmentId,
            };
        }
    }
}
